﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

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
		LEAVING
	};

	[SerializeField] private STATE customer_state = STATE.ENTER;

	private Transform player_transform;

	private new Rigidbody rigidbody;
	private NavMeshPath path;

	private Vector3 goal;
	private Vector3 waypoint;

	[SerializeField] private float hunger = 0.0f;
	[SerializeField] private float hunger_threshold = 15.0f;

	public bool is_ready_for_order {private set; get;}
	private bool was_fed = false;

	[SerializeField] private float waitAfterFood = 10.0f;

	private int food_eaten = 0;
	[SerializeField] private ItemType currentOrder;
	[SerializeField] private List<ItemType> orders = new List<ItemType>();
	[SerializeField] private Transform indicator;
	private GameObject foodModel;
	private MeshRenderer[] modelMeshes;

	public void AteFoodOrder() {
		was_fed = true;
	}

	// Start is called before the first frame update
	void Start() {
		player_transform = GameObject.FindGameObjectWithTag("Player").transform;
		rigidbody = GetComponent<Rigidbody>();

		//TODO: Change the name of the model when we get the new customer model
		modelMeshes = new MeshRenderer[] {
			transform.Find("Model/Character").GetComponent<MeshRenderer>(),
		};
	}

	// Update is called once per frame
	void Update() {
		if (was_fed) {
			customer_state = STATE.FED;
		}

		switch (customer_state) {
			case STATE.ENTER: {
					EnterState();
					break;
				}
			case STATE.FIND_SEAT: {
					FindSeat();
					break;
				}
			case STATE.WAIT_FOR_FOOD: {
					if (orders.Count > 0) {
						currentOrder = orders[0];
						orders.RemoveAt(0);
						hunger = 0;
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
					this.was_fed = false;
					StartCoroutine(Digesting());
					break;
				}
			case STATE.ANGRY: {
					Angry();
					break;
				}
		}
        Angrymeter();
    }

    private void Angrymeter()
    {
        float otherColor = 1f - Mathf.Min(1.0f, hunger / hunger_threshold) * 0.5f;
        foreach (MeshRenderer renderer in modelMeshes)
        {
            renderer.material.color = new Color(1f, otherColor, otherColor, 1f);;
        }
        Debug.Log("Hunger: " + (hunger / hunger_threshold) * 0.5f);
    }

	private IEnumerator Digesting() {
		yield return new WaitForSeconds(waitAfterFood);
		this.customer_state = STATE.WAIT_FOR_FOOD;
	}

	private void PathfindToDestination(Vector3 _goal,float _power) {
		if (path == null) {
			goal = _goal;

			path = new NavMeshPath();
			NavMesh.CalculatePath(transform.position, goal, NavMesh.AllAreas, path);
		}
		else {
			if (path.corners.Length > 1) {
				waypoint = path.corners[1];
			}
			else {
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
	}

	private void FindSeat() {
		// Search for table
		GameObject table = GameObject.FindGameObjectWithTag("Table");

		PathfindToDestination(table.transform.position, 6.0f);

		if (Vector3.Distance(transform.position,goal) < 3.0f) {
			customer_state = STATE.WAIT_FOR_FOOD;
		}
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
		rigidbody.drag = 0.0f;

		Vector3 move_direction = (player_transform.position - rigidbody.transform.position).normalized;

		rigidbody.AddForce(move_direction * Time.deltaTime * (700.0f * rigidbody.mass));
	}

	private void Angrymeter() {
		float otherColor = 1f - Mathf.Min(1.0f, hunger / hunger_threshold) * 0.5f;
		foreach (MeshRenderer renderer in modelMeshes) {
			renderer.material.color = new Color(1f, otherColor, otherColor, 1f);;
		}
		Debug.Log("Hunger: " + (hunger / hunger_threshold) * 0.5f);
	}

	void OnCollisionEnter(Collision col) {
		Debug.Log("FOO");
		Item item = col.transform.GetComponent<Item>();
		if (item != null && item.type == this.currentOrder) {
		Debug.Log("BAR");
			Destroy(col.gameObject);
			this.was_fed = true;
			Destroy(foodModel);
		}
	}
}
