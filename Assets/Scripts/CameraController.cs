using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;

    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2.0f;

    public CinemachineVirtualCamera virtualCamera;

    private float shakeElapsedTime;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public bool Shaking { get; private set; }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update() {
       if (Shaking) {
            shakeElapsedTime -= Time.deltaTime;

            if (shakeElapsedTime <= 0) {
                Shaking = false;
                virtualCameraNoise.m_AmplitudeGain = 0;
                virtualCameraNoise.m_FrequencyGain = 0;
            }
       } 
    }

    public void Shake() {
        Shaking = true;
        shakeElapsedTime = shakeDuration;
        virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = shakeFrequency;
    }
}
