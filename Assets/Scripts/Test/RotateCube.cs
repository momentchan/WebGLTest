using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private float floatingRange = 1f;

    void Start()
    {
        Input.gyro.enabled = true;
        
    }

    void Update()
    {
        transform.rotation = Input.gyro.attitude;
        //transform.Rotate(Vector3.up, rotateSpeed
        transform.position = Vector3.up *  (Mathf.PerlinNoise(Time.time, 0) - 0.5f);
    }

}
