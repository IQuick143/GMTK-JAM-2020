using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TaskManager : MonoBehaviour
{

    [Serializable]
    public class Task
    {
        public float timestamp;
        public TaskEvent task_event;
        public GameObject indicator;
    };

    [SerializeField] private Task current_task;
    [SerializeField] private List<Task> schedule = new List<Task>();
    float current_time = 0.0f;
    int schedule_index = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current_time += Time.deltaTime;

        if (schedule_index + 1 < schedule.Count)
        {
            if (schedule[schedule_index + 1].timestamp <= current_time)
            {
                AdvanceSchedule();
            }
        }
        else
        {
            schedule_index = -1;
            current_time = 0.0f;
        }
    }

    private void AdvanceSchedule()
    {
        schedule_index++;

        current_task = schedule[schedule_index];
        ActivateTask(current_task);
    }

    private void ActivateTask(Task _task)
    {
        _task.indicator.SetActive(true);
    }
}
