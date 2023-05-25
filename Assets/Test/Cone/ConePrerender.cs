using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

public class ConePrerender : Cone
{
    [SerializeField] private List<Texture2DArray> textures;
    [SerializeField] private int frames = 390;
    [SerializeField] private float spotRatio = 0.7f;
    private int id; 

    protected override void Reset()
    {
        base.Reset();

        id = seed < spotRatio ? 0 : 1;
        block.SetTexture("_Textures", textures[id]);
    }

    protected override void Update()
    {
        base.Update();

        block.SetFloat("_Frame", ratio * 390);
        transform.localScale *= id == 0 ? 1 : 2;
    }
}
