using System.Collections;
using System.Collections.Generic;
using mj.gist.postprocessing;
using UnityEngine;

public class ColorMixTest : ImageEffectBase
{
    [SerializeField] private RenderTexture strokeTex;
    protected override void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        material.SetTexture("_StrokeTex", strokeTex);
        material.SetFloat("_StrokeStrength", MicrophoneVolumeDetector.GetVolume() * 0.2f);
        base.OnRenderImage(src, dst);
    }
}
