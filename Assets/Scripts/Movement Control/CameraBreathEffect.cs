using Unity.Cinemachine;
using UnityEngine;

public class CameraBreathEffect : CinemachineExtension
{
    [Header("Position Breathing")]
    [SerializeField] private float posAmplitude = 0.04f;
    [SerializeField] private float posFrequency = 0.7f;

    [Header("Rotation Breathing")]
    [SerializeField] private float rotAmplitude = 3.0f;
    [SerializeField] private float rotFrequency = 0.5f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Aim)
        {
            var posOffset = new Vector3(
                Mathf.Sin(Time.time * posFrequency * 0.5f) * posAmplitude,
                Mathf.Sin(Time.time * posFrequency) * posAmplitude,
                0
            );
            state.PositionCorrection += posOffset;

            var rotOffset = new Vector3(
                Mathf.Sin(Time.time * rotFrequency * 0.8f) * rotAmplitude,
                Mathf.Sin(Time.time * rotFrequency * 0.4f) * rotAmplitude * 0.5f,
                Mathf.Sin(Time.time * rotFrequency * 0.3f) * rotAmplitude * 0.3f
            );
            state.OrientationCorrection *= Quaternion.Euler(rotOffset);
        }
    }
}
