using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    public float velocityThreshold = 1f;
    public float torqueFoce = 1f;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent <Rigidbody>();
    }

    void Update()
    {
        //if (rb.velocity.magnitude < velocityThreshold)
        //{
            this.transform.LookAt(rb.velocity.normalized, Vector3.up);
        //    anim.SetBool("fast", false);
        //}
        //else
        //{
        //    anim.SetBool("fast", true);
        //}
    }
}
