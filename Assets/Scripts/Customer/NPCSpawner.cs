using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customer;
    [SerializeField] private float start_period;
    [SerializeField] private float interval;
    private bool is_first = true;
    private float current_time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current_time += Time.deltaTime;

        if (is_first)
        {
            if (current_time > start_period)
            {
                is_first = false;
                current_time = 0.0f;
                Instantiate(customer, transform.position, new Quaternion());
            }
        }
        else
        {
            if (current_time > interval)
            {
                current_time = 0.0f;
                Instantiate(customer, transform.position, new Quaternion());
            }
        }


    }
}
