using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    [SerializeField] private float spin_speed = 0.3f;
    [SerializeField] private float rise_amount = 0.3f;

    private Vector3 default_pos;
    private float current_time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        default_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        current_time += Time.deltaTime;

        transform.Rotate(0.0f, spin_speed, 0.0f);

        Vector3 set_position = default_pos;
        set_position.y += (Mathf.Sin(current_time) * rise_amount);
        transform.position = set_position;
    }
}
