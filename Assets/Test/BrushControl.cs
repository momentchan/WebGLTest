using System.Collections;
using System.Collections.Generic;
using BezierTools;
using UnityEngine;

public class BrushControl : MonoBehaviour
{
    [SerializeField] Bezier bezier;
    [SerializeField] MeshFilter filter;
    [Range(0,1f)]
    public float t;

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = mesh.uv;

        for (int i = 0; i < uvs.Length; i++)
        {
            var v = vertices[i];
            var wpos = bezier.data.Position(uvs[i].x);
            var lpos = transform.InverseTransformPoint(wpos);
            var wv = transform.TransformPoint(v);
            vertices[i] = lpos;


            Debug.Log($"{uvs[i]} {v} {lpos} {wv}");

            //var v = vertices[i];
            //var lpos = transform.InverseTransformPoint(wpos);
            //lpos.z = 0;
            //vertices[i] += Vector3.up * 0.1f;// lpos;
            //Debug.Log($"{v} {vertices[i]}");
        }
        mesh.vertices = vertices;
        //mesh.RecalculateBounds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bezier.data.Position(t), 0.05f);
    }
}
