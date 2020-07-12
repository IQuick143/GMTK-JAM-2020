using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimation : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent <Rigidbody>();
    }

    void Update()
    {
        this.transform.LookAt(rb.velocity.normalized, Vector3.up);
    }
}
