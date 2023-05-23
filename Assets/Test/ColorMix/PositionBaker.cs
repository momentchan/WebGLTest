using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

public class PositionBaker : MonoBehaviour
{
    [SerializeField] BezierTools.Bezier bezier;
    [SerializeField] private int sampleCount = 100;
    [SerializeField] private float length = 0.1f;
    [SerializeField] private float t;
    [SerializeField] Material mat;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float width = 0.07f;

    public Texture2D rt;

    private void Start()
    {
        rt = new Texture2D(sampleCount, 2, TextureFormat.RGBAFloat, false);
        rt.wrapMode = TextureWrapMode.Clamp;
    }
    Vector3 p1;
    Vector3 p2;

    private void Update()
    {
        t += Time.deltaTime * speed;
        SetPointData();

        p1 = bezier.data.PositionNormalizeT(t % 1);
        p2 = bezier.data.PositionNormalizeT((t + length) % 1);

        mat.SetTexture("_PositionTex", rt);
        mat.SetVector("_P1", p1);
        mat.SetVector("_P2", p2);
        mat.SetFloat("_Width", width);
    }

    public void SetPointData()
    {
        for (int i = 0; i < sampleCount; i++)
        {
            var nt = (t + i * length / sampleCount) % 1;
            var pos = bezier.data.PositionNormalizeT(nt);
            var vel = bezier.data.VelocityNormalizeT(nt);
            rt.SetPixel(i, 0, new Color(pos.x, pos.y, pos.z, 0));
            rt.SetPixel(i, 1, new Color(vel.x, vel.y, vel.z, 0));
        }
        rt.Apply();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(p1, 0.05f);
        Gizmos.DrawWireSphere(p2, 0.05f);
        Gizmos.DrawLine(p1, p1 + bezier.data.VelocityNormalizeT(t % 1) * 0.1f);
        Gizmos.DrawLine(p2, p2 + bezier.data.VelocityNormalizeT((t + length) % 1) * 0.1f);
    }
}
