using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private static HealthManager instance = null;

    public static HealthManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HealthManager();
                
            }
            return instance;
        }
    }
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;

    public ShieldManager shieldManager;

    private void Awake()
    {
        health = maxHealth;
        instance = this;
        shieldManager = new ShieldManager();
    }

    public void Hit(float damage)
    {
        if (shieldManager.Hit(damage))
        {
            DoDamage();
        }
    }

    private void DoDamage(int damage = 1)
    {
        health -= damage;
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {

        }
    }
}
