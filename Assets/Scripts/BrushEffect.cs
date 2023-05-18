using System.Collections;
using System.Collections.Generic;
using mj.gist.postprocessing;
using UnityEngine;

[ExecuteInEditMode]
public class BrushEffect : ImageEffectBase
{
    [SerializeField] CustomRenderTexture rt;
    protected override void Start()
    {
        base.Start();
    }


    protected override void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(rt, dst);
    }
}
