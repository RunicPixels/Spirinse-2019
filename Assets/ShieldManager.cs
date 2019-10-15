using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class ShieldManager
{
    public float shield = 20f;
    public float shieldRechargeRate = 1f;
    public float shieldRechargeDelay = 0.25f;

    private float internalShieldRecharge;
    public void Run()
    {

    }

    public bool Hit(float damage)
    {
        shield -= damage;
        internalShieldRecharge = shieldRechargeDelay;
        if (!(shield < 0f)) return false; // Only Shield Damage

        shield = 0f;
        return true; // Do Health Damage
    }
}
