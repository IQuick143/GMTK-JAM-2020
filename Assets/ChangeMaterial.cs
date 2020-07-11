using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer meshRenderer;
    
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
