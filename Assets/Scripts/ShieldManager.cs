using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class ShieldManager
{
    protected int shield;
    protected int maxShield;

    public ShieldManager(int shieldAmount)
    {
        maxShield = shieldAmount;
        shield = maxShield;
    }

    public void Run()
    {

    }

    public bool Hit(int damage)
    {
        shield -= damage;
        if (!(shield <= 0)) return false;

        shield = 0;
        return true;
    }

    public void RestoreShield(int amount)
    {

    }

    public void IncreaseShield(int amount)
    {

    }
}
