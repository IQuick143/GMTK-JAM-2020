﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CustomerAI : MonoBehaviour {
	[Serializable] public enum STATE {
		ENTER,
		FIND_SEAT,
		WAIT_FOR_FOOD,
		WAITING_FOOD,
		FED,
		DIGESTING,
		HUNGRY,
		ANGRY,
		LEAVING,
        DESTROY
	};

	[SerializeField] private STATE customer_state = STATE.ENTER;

	private Transform player_transform;

	private new Rigidbody rigidbody;
	private NavMeshPath path;

    private bool is_dest_inited = false;
	private Transform chosen_table;
	private bool is_at_table = false;

	private Vector3 goal;
	private Vector3 waypoint;

    private Quaternion rotation;

    [SerializeField] private float hunger = 0.0f;
	[SerializeField] private float hunger_threshold = 15.0f;

	public bool is_ready_for_order {private set; get;}
	private bool was_fed = false;

	[SerializeField] private float waitAfterFood = 20.0f;

	private int food_eaten = 0;
	[SerializeField] private ItemType currentOrder;
	public List<ItemType> orders = new List<ItemType>();
	[SerializeField] private Transform indicator;
	private GameObject foodModel;
	private SkinnedMeshRenderer[] modelMeshes;

    // Audio stuff
    public AudioClip bumpSFX;
    public AudioClip bellSFX;
    public AudioClip orderCompleteSFX;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource walkAudioSource;
    float bounce_cooldown = 0.0f;
    [SerializeField] private float bounce_sound_interval;

	private UIManager uiManager;
	private float goodOrderScore = 2000;

    public void AteFoodOrder() {
		was_fed = true;
	}

	// Start is called before the first frame update
	void Start() {
		player_transform = GameObject.FindGameObjectWithTag("Player").transform;
		rigidbody = GetComponent<Rigidbody>();
		modelMeshes = new SkinnedMeshRenderer[] { transform.Find("Model/Character").GetComponent<SkinnedMeshRenderer>()};
        audioSource = GetComponent<AudioSource>();
		uiManager = FindObjectOfType<UIManager>();
    }

	// Update is called once per frame
	void Update() {
		if (was_fed) {
			customer_state = (customer_state == STATE.ANGRY)?STATE.LEAVING:STATE.FED;
			this.was_fed = false;
		}

		switch (customer_state) {
			case STATE.ENTER: {
					EnterState();
					break;
				}
			case STATE.FIND_SEAT: {
					if (chosen_table == null) {
						// Search for table
						GameObject[] table = GameObject.FindGameObjectsWithTag("Table");

						int selected_table = UnityEngine.Random.Range(0, table.Length);
						is_dest_inited = true;

						chosen_table = table[selected_table].transform;
						goal = chosen_table.position;
					}
					PathfindToDestination(goal, 6.0f);
					if (is_at_table) customer_state = STATE.WAIT_FOR_FOOD;
					break;
				}
			case STATE.WAIT_FOR_FOOD: {
					if (orders.Count > 0) {
						currentOrder = orders[0];
						orders.RemoveAt(0);
						hunger = 0;
                        audioSource.PlayOneShot(bellSFX);
						customer_state = STATE.WAITING_FOOD;
						if (foodModel != null) Destroy(foodModel);
						foodModel = Instantiate(ItemManager.GetModel(currentOrder), indicator);
					} else {
						customer_state = STATE.LEAVING;
					}
					break;
				}
			case STATE.WAITING_FOOD: {
					WaitForFood();
					break;
				}
			case STATE.FED: {
					this.customer_state = STATE.DIGESTING;
                    audioSource.PlayOneShot(orderCompleteSFX);
                    path = new NavMeshPath();
                    StartCoroutine(Digesting());
					break;
				}
            case STATE.ANGRY:
                {
                    path = new NavMeshPath();
                    Angry();
                    break;
                }
            case STATE.LEAVING:
                {
                    Leaving();
                    break;
                }
            case STATE.DESTROY:
                {
                    Destroy(gameObject);
                    break;
                }
        }
		if (bounce_cooldown > 0.0f) {
            bounce_cooldown -= Time.deltaTime;
        }
        Angrymeter();

        if (rigidbody.velocity.magnitude > 0.1f)
        {
            rotation = Quaternion.LookRotation(rigidbody.velocity, transform.up);
            rotation.x = 0.0f;
            rotation.z = 0.0f;
            rigidbody.transform.rotation = rotation;
        }

    }

    private void Angrymeter()
    {
        float otherColor = 1f - Mathf.Min(1.0f, hunger / hunger_threshold) * 0.5f;
        foreach (SkinnedMeshRenderer renderer in modelMeshes)
        {
            renderer.material.color = new Color(1f, otherColor, otherColor, 1f);;
        }
        Debug.Log("Hunger: " + (hunger / hunger_threshold) * 0.5f);
		try {
			if (rigidbody.velocity.magnitude > 1.0f)
			{
				walkAudioSource.UnPause();
			}
			else
			{
				walkAudioSource.Pause();
			}
		} catch(System.Exception) {}

    }

	private IEnumerator Digesting() {
		yield return new WaitForSeconds(waitAfterFood);
		this.customer_state = STATE.WAIT_FOR_FOOD;
	}

	private void PathfindToDestination(Vector3 _goal,float _power) {
        goal = _goal;
        if (path == null || path.corners.Length == 0) {


			path = new NavMeshPath();
			NavMesh.CalculatePath(transform.position, goal, NavMesh.AllAreas, path);
		} else {
			if (path.corners.Length > 1) {
				waypoint = path.corners[1];
			} else {
				waypoint = path.corners[0];
			}

			rigidbody.AddForce((waypoint - transform.position).normalized * _power);

			if (Vector3.Distance(waypoint, transform.position) < 6.0f) {
				rigidbody.AddForce(-rigidbody.velocity);
			}

			if (Vector3.Distance(waypoint, transform.position) < 3.0f) {
				NavMesh.CalculatePath(transform.position, goal, NavMesh.AllAreas, path);
			}
		}
	}

	private void EnterState() {
		customer_state = STATE.FIND_SEAT;
        is_dest_inited = false;
	}

	private void WaitForFood() {
		was_fed = false;
		is_ready_for_order = true;
		PathfindToDestination(goal, 3.0f);

		rigidbody.drag = 0.9f;

		hunger += Time.deltaTime;

		if (hunger > hunger_threshold) {
			customer_state = STATE.ANGRY;
		}
	}

	private void Angry() {
        path = new NavMeshPath();
		rigidbody.drag = 0.0f;

		Vector3 move_direction = (player_transform.position - rigidbody.transform.position).normalized;

		rigidbody.AddForce(move_direction * Time.deltaTime * (700.0f * rigidbody.mass));
	}

	void OnCollisionEnter(Collision col) {
		Item item = col.transform.GetComponent<Item>();
		if (is_ready_for_order && item != null && item.type == this.currentOrder) {
			uiManager.Score += goodOrderScore;
			Destroy(col.gameObject);
			this.was_fed = true;
			Destroy(foodModel);
		}

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if ((col.transform.CompareTag("Player")) && (bounce_cooldown <= 0.0f))
        {
            audioSource.PlayOneShot(bumpSFX);
            bounce_cooldown = bounce_sound_interval;
        }
    }

	void OnTriggerEnter(Collider other) {
		if (other.transform == this.chosen_table) {
			is_at_table = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.transform == this.chosen_table) {
			is_at_table = false;
		}
	}

    private void Leaving()
    {
        // Search for table
        GameObject exit_point = GameObject.FindGameObjectWithTag("Exit");

        PathfindToDestination(exit_point.transform.position, 6.0f);

        if (Vector3.Distance(transform.position, goal) < 3.0f)
        {
            customer_state = STATE.DESTROY;
        }
    }

	public void TimeUp() {
		this.customer_state = STATE.LEAVING;
		this.is_ready_for_order = false;
	}
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(goal, 0.5f);
        Gizmos.DrawSphere(waypoint, 0.2f);
    }
}
