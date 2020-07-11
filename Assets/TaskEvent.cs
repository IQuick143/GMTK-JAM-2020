using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskEvent : MonoBehaviour
{
    [SerializeField] private string tag;

    [SerializeField] private GameObject indicator;
    // Start is called before the first frame update
    private void OnEnable()
    {
        indicator.SetActive(true);
    }

    private void OnDisable()
    {
        indicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider _other)
    {
        if ((_other.tag == tag) && (enabled == true))
        {
            enabled = false;
            Destroy(_other.gameObject);
        }
    }
}
