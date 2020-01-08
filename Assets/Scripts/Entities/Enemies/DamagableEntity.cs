using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;

public class DamagableEntity : MonoBehaviour, IDamagable
{
    private float iFrames;
    /*
    public bool TakeDamage(int damage)
    {
        
        if (iFrames > 0f || cured || damage < 1) return false;
        health -= damage;
        //EffectsManager.Instance.timeManager.Freeze(0.05f, 0, 3f, 3f);
        animator.SetTrigger(Hit);
        hitParticles.Play();

        if (health < 0 && !cured)
        {
            //SpawnEnemy.enemyAmount -= 1;
            //Cure();
        }

        //Stun();
    }
    */
    public bool TakeDamage(int damage)
    {
        return false;
    }
}
