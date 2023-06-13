using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

public class InteractionController : SingletonMonoBehaviour<InteractionController>
{
    [SerializeField] private bool showGUI = true;

    private GUIStyle style;

    private bool supportGyro => SystemInfo.supportsGyroscope;
    public Vector2 GyroAngle => Input.gyro.attitude.eulerAngles;


    
    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 60;
        style.normal.textColor = Color.white;

        Input.gyro.enabled = supportGyro;
    }

    private void OnGUI()
    {
        if (!showGUI) return;

        GUILayout.BeginVertical();
        if (supportGyro)
        {
            GUILayout.Label($"Angle: {Input.gyro.attitude.eulerAngles}", style);
            GUILayout.Label($"Acceleration: {Input.gyro.userAcceleration}", style);
        }
        GUILayout.Label($"Volume: {MicrophoneVolumeDetector.GetVolume()}", style);
        GUILayout.EndVertical();
    }
}
