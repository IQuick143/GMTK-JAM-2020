using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStayOnTable : MonoBehaviour
{
    //When it touches the table, it stays there
    //If it gets pushed by another customer, it leaves the table

    protected Rigidbody rb;
    public float chooseLocationTime;
    protected float currTime;
    protected float lastChooseTime;
    protected Vector3 targetLocation;

    public float forcePower;

    bool sittingOnTable = false;

    public Vector2 minPoint;
    public Vector2 maxPoint;

    protected TableManager tableManager;

    //How much weight the table repulsion has over the random direction vector
    public float tableRepulsionWeight = 2f;

    private float lastTimeTouchPlayer = 0f;
    private float noAvoidTablesTime = 1f;

     [HideInInspector] public static float tableAvoidRadius = 3.75f;

    void Start()
    {
        currTime = lastChooseTime = 0f;
        rb = GetComponent<Rigidbody>();
        ChooseLocation();
        tableManager = FindObjectOfType<TableManager>();
    }

    void Update()
    {
        CheckTime();
    }

    private void FixedUpdate()
    {
        if (!sittingOnTable)
        {
            Move();
        }
    }

    virtual protected void ChooseLocation()
    {
        targetLocation = new Vector3(Random.Range(minPoint.x, maxPoint.x), 0f, Random.Range(minPoint.y, maxPoint.y));
    }

    protected void CheckTime()
    {
        currTime += Time.deltaTime;
        if (!sittingOnTable)
        {
            if (currTime >= lastChooseTime + chooseLocationTime)
            {
                TimePassed();
                lastChooseTime = lastChooseTime + chooseLocationTime;
            }
        }
    }

    virtual protected void TimePassed()
    {
        ChooseLocation();
    }

    private void Move()
    {
        //A blend between a random direction and avoiding the tables
        Vector3 dir = (RemoveY(targetLocation) - RemoveY(transform.position)).normalized;
        if (currTime > lastTimeTouchPlayer + noAvoidTablesTime)
        {
            foreach (Table table in tableManager.tables)
            {
                Vector3 tableCustomerVec = RemoveY(transform.position) - RemoveY(table.transform.position);
                float distFromTable = tableCustomerVec.magnitude;
                if (distFromTable < tableAvoidRadius)
                {
                    rb.AddForce(tableCustomerVec.normalized * rb.velocity.magnitude, ForceMode.VelocityChange);

                    //dir += tableCustomerVec.normalized * tableRepulsionWeight;

                    //The closer from the table you are, the more it pushes
                    //dir += tableCustomerVec.normalized * (1 - (distFromTable / tableAvoidRadius)) * tableRepulsionWeight;
                }
            }
            //dir.Normalize();
        }
        rb.AddForce(dir * forcePower);
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    }

    private Vector3 RemoveY(Vector3 vec)
    {
        return new Vector3(vec.x, 0f, vec.z);
    }

    //If it touches a table
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            sittingOnTable = true;
            rb.velocity = rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            sittingOnTable = false;
            ChooseLocation();
            lastChooseTime = currTime;
        }
        else if (other.CompareTag("Player"))
        {
            lastTimeTouchPlayer = currTime;
        }
    }

    private void OnDrawGizmos()
    {
        //TODO: Make them be at y 1
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetLocation, 0.5f);
        Gizmos.DrawLine(transform.position, targetLocation);
    }
}
