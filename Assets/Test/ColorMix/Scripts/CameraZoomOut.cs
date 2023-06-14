using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    void Update()
    {
        transform.position += Vector3.back * Time.deltaTime * speed;
    }
}
