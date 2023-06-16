using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokeLayerController : MonoBehaviour
{
    [SerializeField] private Vector2 range = new Vector2(1, 3);
    [SerializeField] private List<BezierStrokeGenerator> generators;


    void Update()
    {

        for (var i = 0; i < generators.Count; i++)
        {
            var generator = generators[i];
            generator.transform.localPosition = Vector3.forward * Mathf.Lerp(range.x, range.y, 1f * i / generators.Count);
        }
    }
}
