using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private bool showGUI = true;

    private GUIStyle style;
    void Start()
    {
        style = new GUIStyle();
        Input.gyro.enabled = true;
        style.fontSize = 60;
        style.normal.textColor = Color.white;
    }

    private void OnGUI()
    {
        if (!showGUI) return;

        GUILayout.BeginVertical();
        GUILayout.Label($"Angle: {Input.gyro.attitude.eulerAngles}", style);
        GUILayout.Label($"Acceleration: {Input.gyro.userAcceleration}", style);
        GUILayout.Label($"Volume: {MicrophoneVolumeDetector.GetVolume()}", style);
        GUILayout.EndVertical();
    }
}
