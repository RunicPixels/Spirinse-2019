using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraDistance : MonoBehaviour
{
    public float cameraDistance = 25f;
    public float velocityModifier = 0.3f;
    
    private float baseDistance;
    private float velocityDistance = 0;
    private float newDistance = 0;
    private float swapTime;

    private void Awake()
    {
        newDistance = cameraDistance;
        baseDistance = cameraDistance;
    }
    // Update is called once per frame
    private void Update()
    {
        var position = transform.position;
        var distance = cameraDistance;
        distance = Mathf.Lerp(distance, newDistance, swapTime) + velocityDistance;
        if (swapTime < 1f)
        {
            swapTime += Time.deltaTime;
            if (swapTime > 1f) swapTime = 1f;
        }
        position = new Vector3(position.x, distance / 5.33f, -distance);
        transform.position = position;
    }

    public void SetVelocityDistance(float velocity)
    {
        var oldDistance = velocityDistance;
        var newDistance = velocity * velocityModifier;
        var clampAmount = 1f * Time.deltaTime;
        velocityDistance = Mathf.Clamp(newDistance, oldDistance - clampAmount, oldDistance + clampAmount);
    }

    public void SetNewCameraDistance(float distance)
    {
        cameraDistance = newDistance;
        swapTime = 0;
        newDistance = distance;
    }

    public float GetBaseDistance()
    {
        return baseDistance;
    }

}
