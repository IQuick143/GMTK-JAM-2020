using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private List<Camera> cameras = new List<Camera>();
    [SerializeField] private float camera_time = 8.0f;
    private float current_time = 0.0f;
    private int camera_index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current_time += Time.deltaTime;

        if (current_time > camera_time)
        {
            current_time = 0.0f;

        }
    }
}
