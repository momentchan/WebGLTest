using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeGenerator : MonoBehaviour
{
    [SerializeField] private Cone cone;
    [SerializeField] private int count = 20;

    private List<Cone> cones = new List<Cone>();

    public Vector2 scaleX = new Vector2(0.5f, 1f);
    public Vector2 scaleY = new Vector2(0.5f, 1f);

    public Vector2 shift = new Vector2(0, 0.02f);
    public Vector2 radiusMin = new Vector2(0, 0.04f);
    public Vector2 radiusMax = new Vector2(0.04f, 0.18f);
    public Vector2 decayPower = new Vector2(0.5f, 4f);
    public Vector2 frequency = new Vector2(0f, 60f);
    public Vector2 duration = new Vector2(2f, 4f);
    public Vector2 rotation = new Vector2(-60, -120);
    public Color color1, color2;
    public AnimationCurve curve;

    public Vector3 GetRandomPosition()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value, 0.5f));
    }

    void Start()
    {
        for (var i = 0; i < count; i++)
        {
            var c = Instantiate(cone, transform);
            c.Setup(this);
            cones.Add(c);
        }
    }
}
