using System;
using System.Collections;
using System.Collections.Generic;
using Spirinse.System;
using UnityEngine;

public class OverrideCamera : MonoBehaviour
{
    public enum CameraMode { FollowPlayer, StaticTarget, StaticLookAtPlayer, Inbetween }

    private CameraMode _mode;
    public CameraMode Mode
    {
        get => _mode;
        set
        {
            _mode = value;
            SwapEnemMode();
        }
        

    }
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

    public void SwapEnemMode()
    {
        switch (_mode)
        {
            case CameraMode.FollowPlayer :
                distance = 30;
                break;
            default :
                distance = 20;
                break;
        }
    }
}
