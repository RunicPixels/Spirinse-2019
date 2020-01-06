using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySwap : MonoBehaviour
{
    public GameObject swappedObject;
    
    public bool inheritScale = true;

    private bool isQuitting = false;
    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    public void OnDestroy()
    {
        if (isQuitting) return;
        var obj = Instantiate(swappedObject);
        obj.transform.position = transform.position;
        obj.SetActive(true);
        if (inheritScale)
        {
            obj.transform.localScale = transform.lossyScale;
            
        }
        obj.transform.localRotation = transform.localRotation;
    }
}
