using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

public class WindSound : MonoBehaviour
{
    public int objectsInWind;
    [EventRef] public string windEventRef;
    public EventInstance windEventInstance;
    
    private PARAMETER_ID windParameterId;
    private FMOD.Studio.EventDescription windEventDescription;
    private FMOD.Studio.PARAMETER_DESCRIPTION windParameterDescription;
    

    
    
    // Start is called before the first frame update
    private void Start()
    {
        windEventDescription.getParameterDescriptionByName("InWind", out windParameterDescription);
        windParameterId = windParameterDescription.id;
        windEventInstance = FMODUnity.RuntimeManager.CreateInstance(windEventRef);
        
        windEventInstance.getDescription(out windEventDescription);
        windEventInstance.start();
    }


    private void OnEnable()
    {
        windEventInstance.start();
    }

    private void OnDisable()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>())
        {
            objectsInWind += 1;
            SetParams();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() && objectsInWind > 0)
        {
            objectsInWind -= 1;
            SetParams();
        }
    }

    private void SetParams()
    {
        int windValue = Mathf.RoundToInt(Mathf.Min(1f, objectsInWind));
        windEventInstance.setParameterByName("InWind", windValue);
    }
}
