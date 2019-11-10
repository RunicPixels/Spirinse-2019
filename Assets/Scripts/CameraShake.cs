using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    private void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            float modifier = Mathf.Max(shakeDuration, 1f);
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount * modifier;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    public void DoShake(float intensity, float duration, float maxIntensityModifier = 1f, float maxDurationModifier = 1f, bool addDuration = false)
    {
        float shakeDurationAddition = addDuration ? 1 : 0;

        shakeAmount = Mathf.Clamp(intensity + shakeAmount, shakeAmount, intensity * maxIntensityModifier);
        shakeDuration = Mathf.Clamp(duration + shakeDuration * shakeDurationAddition, shakeDuration, duration * maxDurationModifier);
    }
}