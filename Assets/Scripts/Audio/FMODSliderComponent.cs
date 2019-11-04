using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class FMODSliderComponent : MonoBehaviour
{

    [HideInInspector] public float value;

    [EventRef]
    public string Event = "";


    [ SerializeField][Range(0f,4.1f)] private float parameter;


    public float Parameter
    {
        get => parameter;
        set
        {
            parameter = value;
            UpdateParam();
        }
    }

    FMOD.Studio.EventInstance rain;
    FMOD.Studio.EventDescription rainEventDescription;

    FMOD.Studio.PARAMETER_DESCRIPTION rainParameterDescription;
    FMOD.Studio.PARAMETER_ID rainOnParameterId;

    private void Awake()
    {
        rain = FMODUnity.RuntimeManager.CreateInstance(Event);
        rainEventDescription = FMODUnity.RuntimeManager.GetEventDescription(Event);
        rain.getDescription(out rainEventDescription);
        rainEventDescription.getParameterDescriptionByName("Intensity Battle", out rainParameterDescription);
        rain.start();
        rainOnParameterId = rainParameterDescription.id;
    }

    public void Update()
    {
        UpdateParam();
    }
    [ClickableFunction] public void UpdateParam()
    {
        rain.setParameterByName("RainIntensity", parameter);
    }
}