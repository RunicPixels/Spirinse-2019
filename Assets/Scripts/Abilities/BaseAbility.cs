using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Spirinse.Interfaces;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour, IAbility
{
    [SerializeField]
    protected float duration;

    protected float currentDuration;

    public Action OnAbilityUse;
    public Action OnAbilityStay;
    public Action OnAbilityExit;

    public virtual void Play()
    {
        currentDuration = duration;
        OnAbilityUse?.Invoke();
        Debug.Log("Invoking Event");
    }

    public virtual bool Run()
    {
        if (currentDuration > 0f)
        {
            currentDuration -= Time.deltaTime;
            return true;
        }
        
        OnAbilityStay?.Invoke();
        
        return false;
    }

    public virtual void Stop()
    {
        OnAbilityExit?.Invoke();
    }
}
