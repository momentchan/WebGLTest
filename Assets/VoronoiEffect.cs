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

    private RenderTexture voronoi;
    private Texture2D pointTex;

    private int w = 2048;
    private int h = 2048;

    private void Start()
    {
        voronoi = RTUtil.Create(w, h, 0, RenderTextureFormat.ARGBFloat, false, false, false, TextureWrapMode.Clamp, FilterMode.Point);
        pointTex = new Texture2D(points.Count, 2, TextureFormat.RGBAFloat, false);
        pointTex.filterMode = FilterMode.Point;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetPointData();
        effect.SetInt("_PointCount", points.Count);
        effect.SetTexture("_PointTex", pointTex);

        effect.SetTexture("_VoronoiTex", voronoi);

        Graphics.Blit(null, voronoi, effect, 0);

        Graphics.Blit(voronoi, destination, effect, 1);
    }

    public void SetPointData()
    {
        for (int i = 0; i < points.Count; i++)
        {
            var p = points[i];
            var pos = Camera.main.WorldToViewportPoint(p.pos.position);
            pointTex.SetPixel(i, 0, new Color(pos.x, pos.y, 0, 0));
            pointTex.SetPixel(i, 1, p.color);
        }

        pointTex.Apply();
    }

    [System.Serializable]
    public class Point
    {
        public Transform pos;
        public Color color;
    }
}
