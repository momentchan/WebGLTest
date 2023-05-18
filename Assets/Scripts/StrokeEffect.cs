using mj.gist;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class StrokeEffect : MonoBehaviour
{
    [SerializeField] public Material effect;
    [SerializeField] public Material output;
    [SerializeField] private float strokeLength = 600f;
    [SerializeField] private float brushSize = 20f;
    [SerializeField] private Vector2Int resolution = new Vector2Int(512, 512);

    private PingPongRenderTexture rt;
    private Vector4 brushData;

    void Start()
    {
        rt = new PingPongRenderTexture(resolution.x, resolution.y, 0, RenderTextureFormat.ARGBFloat, false);
    }

    void Update()
    {
        effect.SetFloat("_BrushSize", brushSize);
        effect.SetVector("_TexSize", new Vector4(resolution.x, resolution.y, 1f / resolution.x, 1f / resolution.y));
        effect.SetVector("_BrushData", brushData);

        var mouse = new Vector4(
            Input.mousePosition.x / Screen.width * resolution.x,
            Input.mousePosition.y / Screen.height * resolution.y,
            Input.GetMouseButton(0) ? 1.0f : 0.0f,
            Input.GetMouseButton(1) ? 1.0f : 0.0f);

        effect.SetVector("_Mouse", mouse);

        Graphics.Blit(rt.Read, rt.Write, effect);

        UpdateBrush(mouse);

        rt.Swap();
    }

    private void UpdateBrush(Vector4 mouse)
    {
        var strength = brushData.w;
        strength -= Vector2.Distance(mouse, new Vector2(brushData.x, brushData.y)) / strokeLength;
        if (mouse.z != brushData.z)
        {
            strength = mouse.z;
        }
        strength = Mathf.Clamp01(strength);

        brushData = new Vector4(mouse.x, mouse.y, mouse.z, strength);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        output.SetTexture("_Mask", rt.Read);
        Graphics.Blit(null, destination, output);
    }
}
