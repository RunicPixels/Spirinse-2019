using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using DefaultNamespace;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IPlayerProjectile
{
    public float damage = 6;

    public float energySteal = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetDamage()
    {
        Controls.chi += energySteal;
        return damage;
    }
}
