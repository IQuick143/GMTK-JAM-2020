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
        Vector3 dir = (targetLocation - transform.position).normalized;
        float tableRepulshionWeight = 2f;//We'll give priority to the tables over the random direction
        float tableRepulsionRadius = 3.4f;
        foreach (Table table in tableManager.tables)
        {
            Vector3 tableRepulsionVec = transform.position - table.transform.position;
            //float dir
            if (tableRepulsionVec.sqrMagnitude < tableRepulsionRadius * tableRepulsionRadius)
            {
                //The closer from the table you are, the more it pushes
                //dir += tableRepulsionVec.normalized * (1 - (tableRepulsionRadius));
            }
        }
        dir.Normalize();
        rb.AddForce(dir * forcePower);
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
    }
}
