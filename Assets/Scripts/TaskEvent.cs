using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskEvent : MonoBehaviour
{
    [SerializeField] private Collider col;
    [SerializeField] private string tag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == tag)
        {
            gameObject.SetActive(false);
        }
    }
}
