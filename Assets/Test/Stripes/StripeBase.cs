using mj.gist;
using UnityEngine;

public class StripeBase : MonoBehaviour
{
    protected Block block;
    protected StripeControllerBase controller;

    public float seed;
    public int id;
    public int type;

    internal void Setup(StripeControllerBase controller, int id, int type, Material mat)
    {
        this.controller = controller;
        this.id = id;
        this.type = type;   
        this.GetComponent<Renderer>().material = mat;
        seed = Random.value;
    }

    void Start()
    {
        block = new Block(GetComponent<Renderer>());
        block.SetFloat("_Seed", Random.value);
        block.Apply();
    }
    protected virtual void Update()
    {
        transform.localScale = controller.GetScale(seed, type);

        var p = controller.GetPosition(this);
        p += Vector3.up * (-transform.localScale.y * 0.5f + controller.RandomOffset(type) * seed);
        transform.localPosition = p;
    }
}
