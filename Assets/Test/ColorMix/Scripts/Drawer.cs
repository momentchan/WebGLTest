using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.2f;
    [SerializeField] private float border = 0.1f;
    private Rigidbody2D rb;
    private Vector3 vel;
    public Bounds bounds;

    public float Speed => rb.velocity.magnitude;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bounds.SetMinMax(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)),
                         Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)));
    }

    void Update()
    {

        vel = Input.acceleration * moveSpeed;
        rb.velocity = vel;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bounds.min.x + border, bounds.max.x - border),
                                         Mathf.Clamp(transform.position.y, bounds.min.y + border, bounds.max.y - border),
                                         0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(bounds.center, bounds.size - Vector3.one * border * 2);
    }
}
