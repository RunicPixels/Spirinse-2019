using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Spirinse.Interfaces;
using UnityEngine;

public class BaseAttack : BaseAbility, IAttack
{
    public int damage = 6;
    public float energySteal = 0f;

    public override void Play()
    {
        base.Play();
    }

    public override bool Run()
    {
        return base.Run();
    }

    public override void Stop()
    {
        base.Stop();
    }

    public int DoAttack()
    {
        Controls.chi += energySteal;
        return damage;
    }
}
