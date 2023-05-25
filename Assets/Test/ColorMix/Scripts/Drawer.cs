using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.2f;
    [SerializeField] private float padding = 0.05f;
    private Rigidbody2D rb;
    private Vector3 vel;
    public Bounds bounds;

    public float Speed => rb.velocity.magnitude;
    public Vector3 Vel => rb.velocity;

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

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bounds.min.x - padding, bounds.max.x + padding),
                                         Mathf.Clamp(transform.position.y, bounds.min.y - padding, bounds.max.y + padding),
                                         0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(bounds.center, bounds.size + Vector3.one * padding * 2);
    }
}
