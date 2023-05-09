using System;
using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class StrokeEffectCS : MonoBehaviour
{
    [SerializeField] private ComputeShader cs;

    [SerializeField] public Material output;
    [SerializeField] private Vector2Int resolution = new Vector2Int(512, 512);

    private PingPongRenderTexture rt;
    private GraphicsBuffer brushData;
    private float[] d = new float[4];
    void Start()
    {
        var f1 = SystemInfo.supportsComputeShaders;

        rt = new PingPongRenderTexture(resolution.x, resolution.y, 0, GraphicsFormat.R32G32B32A32_SFloat);
        brushData = new GraphicsBuffer(GraphicsBuffer.Target.Structured, 1, sizeof(float) * 4);
    }

    void Update()
    {
        cs.SetTexture(0, "_MainTex", rt.Read);
        cs.SetTexture(0, "_Result", rt.Write);

        cs.SetVector("_TexSize", new Vector4(resolution.x, resolution.y, 1f / resolution.x, 1f / resolution.y));

        cs.SetVector("_Mouse", new Vector4(
            Input.mousePosition.x / Screen.width * resolution.x,
            Input.mousePosition.y / Screen.height * resolution.y,
            Input.GetMouseButton(0) ? 1.0f : 0.0f,
            Input.GetMouseButton(1) ? 1.0f : 0.0f
        ));
        cs.SetBuffer(0, "_BrushData", brushData);
        cs.Dispatch(0, 512 / 8, 512 / 8, 1);


        rt.Swap();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        output.SetTexture("_Mask", rt.Read);
        Graphics.Blit(null, destination, output);
    }
}
