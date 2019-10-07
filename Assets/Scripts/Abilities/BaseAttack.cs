using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using DefaultNamespace;
using UnityEngine;

public class BaseAttack : BaseAbility, IAttack
{
    public float damage = 6;
    public float energySteal = 0f;

    public override void Execute()
    {
        DealDamage();
    }

    public float DealDamage()
    {
        Controls.chi += energySteal;
        return damage;
    }
}
