using mj.gist;
using UnityEngine;

public class Stripe : MonoBehaviour
{
    [SerializeField] Block block;
    [SerializeField] StripeController controller;

    private float seed;
    private int id;
    internal void Setup(StripeController controller, int id)
    {
        this.controller = controller;
        this.id = id;
        seed = Random.value;
    }

    void Start()
    {
        block = new Block(GetComponent<Renderer>());
        block.SetFloat("_Seed", Random.value);
        block.Apply();
    }

    void Update()
    {
        transform.localScale = controller.scale * controller.scaleRandom.Lerp(seed);

        var p = controller.GetPosition(id);
        p += Vector3.up * (-transform.localScale.y * 0.5f + controller.randomOffset * seed);
        transform.localPosition = p;
    }
}
