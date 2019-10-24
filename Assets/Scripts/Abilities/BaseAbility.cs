using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour, IAbility
{
    [SerializeField]
    protected float duration;

    protected float currentDuration;
    public virtual void Play()
    {
        currentDuration = duration;
    }

    public virtual bool Run()
    {
        if (currentDuration > 0f)
        {
            currentDuration -= Time.deltaTime;
            return true;
        }

        return false;
    }

    public virtual void Stop()
    {

    }
}
