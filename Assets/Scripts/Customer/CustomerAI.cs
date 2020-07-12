using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class CustomerAI : MonoBehaviour
{
    [Serializable] public enum STATE { ENTER,FIND_SEAT,WAITING_FOOD,FED,HUNGRY,ANGRY,LEAVING};

    [SerializeField] private STATE customer_state = STATE.ENTER;

    private Transform player_transform;

    private Rigidbody rigidbody;
    private NavMeshPath path;

    private Vector3 goal;
    private Vector3 waypoint;

    [SerializeField] private float hunger = 0.0f;
    [SerializeField] private float hunger_threshold = 15.0f;

    private bool is_ready_for_order = false;
    private bool was_fed = false;

    private int food_eaten = 0;
    [SerializeField] private int max_food = 3;

    private MeshRenderer[] modelMeshes;

    public bool GetIsOrderReady()
    {
        return is_ready_for_order;
    }

    public void AteFoodOrder()
    {
        was_fed = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        player_transform = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = GetComponent<Rigidbody>();

        //TODO: Change the name of the model when we get the new customer model
        modelMeshes = new MeshRenderer[] {
            transform.Find("Model/Face").GetComponent<MeshRenderer>(),
            transform.Find("Model/Body").GetComponent<MeshRenderer>()
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (was_fed)
        {
            customer_state = STATE.FED;
        }

        switch (customer_state)
        {
            case STATE.ENTER:
                {
                    EnterState();
                    break;
                }
            case STATE.FIND_SEAT:
                {
                    FindSeat();
                    break;
                }
            case STATE.WAITING_FOOD:
                {
                    WaitForFood();
                    break;
                }
            case STATE.FED:
                {
                    Fed();
                    break;
                }
            case STATE.ANGRY:
                {
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

    private void PathfindToDestination(Vector3 _goal,float _power)
    {
        if (path == null)
        {
            goal = _goal;

            path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, goal, NavMesh.AllAreas, path);
        }
        else
        {
            if (path.corners.Length > 1)
            {
                waypoint = path.corners[1];
            }
            else
            {
                waypoint = path.corners[0];
            }

            rigidbody.AddForce((waypoint - transform.position).normalized * _power);

            if (Vector3.Distance(waypoint, transform.position) < 6.0f)
            {
                rigidbody.AddForce(-rigidbody.velocity);
            }

            if (Vector3.Distance(waypoint, transform.position) < 3.0f)
            {
                NavMesh.CalculatePath(transform.position, goal, NavMesh.AllAreas, path);
            }
        }
    }

    private void EnterState()
    {
        customer_state = STATE.FIND_SEAT;
    }

    private void FindSeat()
    {
        // Search for table
        GameObject table = GameObject.FindGameObjectWithTag("Table");

        PathfindToDestination(table.transform.position,6.0f);

        if (Vector3.Distance(transform.position,goal) < 3.0f)
        {
            customer_state = STATE.WAITING_FOOD;
        }
    }

    private void WaitForFood()
    {
        was_fed = false;
        is_ready_for_order = true;
        PathfindToDestination(goal, 3.0f);

        rigidbody.drag = 0.9f;

        hunger += Time.deltaTime;

        if (hunger > hunger_threshold)
        {
            customer_state = STATE.ANGRY;
        }
    }

    private void Fed()
    {
        hunger = 0.0f;
        was_fed = false;
        is_ready_for_order = false;

        food_eaten++;

        if (food_eaten > max_food)
        {
            customer_state = STATE.LEAVING;
        }
        else
        {
            customer_state = STATE.FIND_SEAT;
        }
    }

    private void Angry()
    {
        rigidbody.drag = 0.0f;

        Vector3 move_direction = (player_transform.position - rigidbody.transform.position).normalized;

        rigidbody.AddForce(move_direction * Time.deltaTime * (700.0f * rigidbody.mass));
    }
}
