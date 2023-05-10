using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using mj.gist;
using UnityEngine;

public class VoronoiEffect : MonoBehaviour
{
    [SerializeField] public Material effect;

    [SerializeField] private List<Point> points;

    private GraphicsBuffer pointBuffer;

    private RenderTexture voronoi;

    private int w = 2048;
    private int h = 2048;

    private void Start()
    {
        pointBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 5, Marshal.SizeOf(typeof(PointData)));
        voronoi = RTUtil.Create(w, h, 0, RenderTextureFormat.ARGBFloat, true, false, false, TextureWrapMode.Clamp, FilterMode.Point);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var pd = points.Select(p =>
         new PointData()
         {
             pos = Camera.main.WorldToViewportPoint(p.pos.position),
             color = p.color
         }).ToArray();

        pointBuffer.SetData(pd);

        effect.SetBuffer("_PointBuffer", pointBuffer);
        effect.SetInt("_PointCount", points.Count);


        Graphics.Blit(null, voronoi, effect, 0);

        effect.SetTexture("_VoronoiTex", voronoi);

        Graphics.Blit(voronoi, destination, effect, 1);
    }

    [System.Serializable]
    public struct PointData
    {
        public Vector2 pos;
        public Color color;
    }


    [System.Serializable]
    public class Point
    {
        public Transform pos;
        public Color color;
    }
}
