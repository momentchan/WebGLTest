using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackgroundLayer : MonoBehaviour
{
    [SerializeField] private Camera cam;

    void Update()
    {
        var d = transform.localPosition.z;

        var h = 2 * Mathf.Tan(Mathf.Deg2Rad * cam.fieldOfView * 0.5f) * d;
        var w = h * cam.aspect;
        transform.localScale = new Vector3(w, h, 1);
    }
}
