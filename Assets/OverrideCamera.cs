using System;
using System.Collections;
using System.Collections.Generic;
using Spirinse.System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class OverrideCamera : MonoBehaviour
{
    public enum CameraMode { FollowPlayer, StaticTarget, StaticLookAtPlayer, Inbetween }


    public CameraMode ModePreset;
    
    private CameraManager _cameraManager;

    public float distance = 50;
    private float oldDistance;

    #if UNITY_EDITOR
    private void OnEnable()
    {
        EditorApplication.update += SwapEnemMode;
    }

    private void OnDisable()
    {
        EditorApplication.update -= SwapEnemMode;
    }
    #endif
    
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
        switch (ModePreset)
        {
            case CameraMode.StaticTarget :
                distance = 30;
                break;
            default :
                distance = 20;
                break;
        }
    }
}
