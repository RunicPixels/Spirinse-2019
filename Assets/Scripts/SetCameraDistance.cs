using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraDistance : MonoBehaviour
{
    private float baseDistance = 4f;
    public float cameraDistance = 25f;

    public float velocityModifier = 0.3f;
    private float velocityDistance = 0;

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        var distance = cameraDistance + velocityDistance;
        position = new Vector3(position.x,position.y,-distance);
        transform.position = position;
    }
    public void SetVelocityDistance(float velocity)
    {
        velocityDistance = velocity * velocityModifier;
    }
}
