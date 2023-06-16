using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGyroControl : MonoBehaviour
{
    [SerializeField] private Vector3 focusPosition = Vector3.forward;

    [SerializeField] private Vector2 angleRange = new Vector2(30f, 20f);
    [SerializeField] private Vector2 offset = Vector2.one;
    [SerializeField] private Vector2 testAngle = new Vector2(0, 270f);

    private Camera cam;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
    }

    void UpdateCameraPos(Vector2 angle)
    {
        var pos = cam.transform.position;

        var sign = angle.x > 180 ? -1f : 1f;
        var diff_x = sign > 0 ? angle.x : angle.x - 360;
        var x = sign * Mathf.Clamp01(Mathf.Abs(diff_x) / angleRange.x) * offset.x;

        var diff_y = angle.y - 270f;
        var y = Mathf.Sign(diff_y) * Mathf.Clamp01(Mathf.Abs(diff_y) / angleRange.y) * offset.y;


        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(x, y, pos.z), 0.5f);
    }

    void Update()
    {
#if !UNITY_EDITOR
        UpdateCameraPos(InteractionController.Instance.GyroAngle);
#else
        UpdateCameraPos(testAngle);
#endif
        cam.transform.LookAt(focusPosition);
    }
}
