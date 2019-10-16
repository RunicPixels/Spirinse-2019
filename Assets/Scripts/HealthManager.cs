using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private static HealthManager instance = null;

    public static HealthManager Instance => instance;
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int healthCap;

    protected ShieldManager shieldManager;

    private void Awake()
    {
        health = maxHealth;
        instance = this;
        shieldManager = new ShieldManager(maxHealth);
    }

    public void Hit(int damage)
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

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void IncreaseHealth(int amount, bool doHeal = true)
    {
        maxHealth += amount;
        if (doHeal) health += amount;
    }
}
