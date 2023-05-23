using System.Collections;
using System.Collections.Generic;
using mj.gist.postprocessing;
using UnityEngine;

public class ConeTest : ImageEffectBase
{
    [SerializeField] private Material blurMat;
    [SerializeField] private int lod = 2;
    [SerializeField] private int nIterations=2;

    protected override void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        var blur = RenderTexture.GetTemporary(src.descriptor);
        BlurUtil.BlurWithDownSample(src, blur, lod, nIterations, blurMat);

        material.SetTexture("_BlurTex", blur);
        Graphics.Blit(src, dst, material);

        RenderTexture.ReleaseTemporary(blur);
    }
}
