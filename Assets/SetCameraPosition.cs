using System;
using System.Collections;
using System.Collections.Generic;
using Spirinse.System;
using UnityEngine;

public class SetCameraPosition : MonoBehaviour
{
    private CameraManager _cameraManager;

    public float distance = 50;
    private float oldDistance;

    private void Start()
    {
        _cameraManager = CameraManager.Instance;
        oldDistance = _cameraManager.cameraDistance.GetBaseDistance();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Defender"))
        {
            _cameraManager.cameraDistance.SetNewCameraDistance(distance);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Defender"))
        {
            _cameraManager.cameraDistance.SetNewCameraDistance(oldDistance);
        }
    }
}
