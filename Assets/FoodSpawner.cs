using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodSpawner : MonoBehaviour
{
    [Serializable]
    public class FoodSpawnpoint
    {
        public GameObject food;
        public Transform spawnpoint;

        public float interval;
        public float current_time;
    }

    [SerializeField] private List<FoodSpawnpoint> spawnpoints = new List<FoodSpawnpoint>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var spawnpoint in spawnpoints)
        {
            spawnpoint.current_time += Time.deltaTime;

            if (spawnpoint.current_time > spawnpoint.interval)
            {
                Instantiate(spawnpoint.food, spawnpoint.spawnpoint.position,new Quaternion());

                spawnpoint.current_time = 0.0f;
            }
        }
    }
}
