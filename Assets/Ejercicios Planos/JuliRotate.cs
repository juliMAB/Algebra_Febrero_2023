using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JuliRotate : MonoBehaviour
{
    public GameObject go;
    public float speed;
    void Update()
    {
        go.transform.Rotate(Vector3.up*speed*Time.deltaTime);
    }
}
