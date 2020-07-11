using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour//TODO: Entity
{
    private Rigidbody rb;
    public float chooseLocationTime;
    private float currTime;
    private float lastChooseTime;
    private Vector3 location;
    public Vector2 minPoint;
    public Vector2 maxPoint;

    public float forcePower;

    void Start()
    {
        currTime = lastChooseTime = 0f;
        rb = GetComponent<Rigidbody>();
        ChooseLocation();
    }

    void Update()
    {
        CheckTime();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ChooseLocation()
    {
        location = new Vector3(Random.Range(minPoint.x, maxPoint.x), 0f, Random.Range(minPoint.y, maxPoint.y));
    }

    private void CheckTime()
    {
        currTime += Time.deltaTime;
        if (currTime >= lastChooseTime + chooseLocationTime)
        {
            ChooseLocation();
            lastChooseTime = lastChooseTime + chooseLocationTime;
        }
    }

    private void Move()
    {
        Vector3 dir = (location - transform.position).normalized;
        rb.AddForce(dir * forcePower);
    }
}
