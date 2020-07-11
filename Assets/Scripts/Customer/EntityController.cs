using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    protected Rigidbody rb;
    public float chooseLocationTime;
    protected float currTime;
    protected float lastChooseTime;
    protected Vector3 targetLocation;

    public float forcePower;

    public AudioClip bumpSFX;
    private AudioSource audioSource;

    protected void Start()
    {
        currTime = lastChooseTime = 0f;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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

    virtual protected void ChooseLocation()
    {}

    protected void CheckTime()
    {
        currTime += Time.deltaTime;
        if (currTime >= lastChooseTime + chooseLocationTime)
        {
            TimePassed();
            lastChooseTime = lastChooseTime + chooseLocationTime;
        }
    }

    virtual protected void TimePassed()
    {}

    private void Move()
    {
        Vector3 dir = (targetLocation - transform.position).normalized;
        rb.AddForce(dir * forcePower);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            audioSource.PlayOneShot(bumpSFX);
        }
    }
}
