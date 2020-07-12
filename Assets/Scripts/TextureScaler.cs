using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScaler : MonoBehaviour
{
    [SerializeField] private float tileX = 1;
    [SerializeField] private float tileY = 1;
    
    private Mesh mesh;
    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        mat.mainTextureScale = new Vector2(
            (mesh.bounds.size.x * Math.Max(transform.localScale.x, transform.localScale.z)) * tileX,
            (mesh.bounds.size.y * transform.localScale.y) * tileY);
    }
}
