using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthManager : MonoBehaviour
{
    private static HealthManager instance = null;

    public static HealthManager Instance => instance;

    [SerializeField] protected int _health;
    protected int Health
    {
        set
        {
            _health = value;
            ChangeHealthEvent?.Invoke(_health);

        }
        get => _health;
    }

    [SerializeField] protected int _maxHealth;
    protected int MaxHealth
    {
        set
        {
            _maxHealth = value;
            ChangeMaxHealthEvent?.Invoke(_maxHealth);
        }
        get => _maxHealth;
    }
    [SerializeField] protected int healthCap;
    public int GetHealthCap => healthCap;

    protected ShieldManager shieldManager;

    public Action<int> ChangeHealthEvent;
    public Action<int> ChangeMaxHealthEvent;

    private void Awake()
    {
        ChangeHealthEvent += PlaceholderMethod;
        ChangeMaxHealthEvent += PlaceholderMethod;
        Health = MaxHealth;
        instance = this;
        shieldManager = new ShieldManager(MaxHealth);
    }

    public void PlaceholderMethod(int i)
    {
        Debug.Log(i);
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
        Health -= damage;
        ChangeHealthEvent.Invoke(Health);
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (Health <= 0)
        {

        }
    }

    public void Heal(int amount)
    {
        Health += amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public void IncreaseHealth(int amount, bool doHeal = true)
    {
        MaxHealth += amount;
        if (doHeal) Health += amount;
    }

    [ClickableFunction]
    public void DebugUpdateHealth()
    {
        UIManager.Instance.GetHealthUI.SetMaxHealthContainers(healthCap);
        ChangeHealthEvent.Invoke(Health);
        ChangeMaxHealthEvent.Invoke(MaxHealth);
    }
}
