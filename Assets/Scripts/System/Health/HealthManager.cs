using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UIElements;
using Spirinse.Player;

namespace Spirinse.System.Health
{
    public class HealthManager : MonoBehaviour
    {
        private static HealthManager instance = null;
        public static HealthManager Instance => instance;

        [SerializeField] protected int _health;
        protected int Health {
            set {
                _health = value;
                ChangeHealthEvent?.Invoke(_health);

            }
            get => _health;
        }

        [SerializeField] protected int _maxHealth;
        protected int MaxHealth {
            set {
                _maxHealth = value;
                ChangeMaxHealthEvent?.Invoke(_maxHealth);
            }
            get => _maxHealth;
        }
        [SerializeField] protected int healthCap;
        public int GetHealthCap => healthCap;

        [SerializeField] protected ShieldManager shieldManager;
        public ShieldManager ShieldManager => shieldManager;

        public Action<int> ChangeHealthEvent;
        public Action<int> ChangeMaxHealthEvent;

        private void Awake()
        {
            ChangeHealthEvent += CheckDeath;
            ChangeMaxHealthEvent += CheckDeath;

            Health = MaxHealth;
            instance = this;

            InitShield();
        }

        [ClickableFunction]
        public void InitHealth()
        {
            UI.UIManager.Instance.GetHealthUI.SetMaxHealthContainers(healthCap);
            ChangeHealthEvent.Invoke(Health);
            ChangeMaxHealthEvent.Invoke(MaxHealth);
        }
        [ClickableFunction]
        public void InitShield()
        {
            if (!shieldManager) Debug.LogError("No ShieldManager Assigned!");
            shieldManager.SetNewMaxShield(MaxHealth);
            shieldManager.SetShieldCap(healthCap);
            shieldManager.SetShield(MaxHealth);
            shieldManager.InitShield();
        }

        public void Hit(int damage, CharacterType hitType)
        {
            switch (hitType)
            {
                case CharacterType.Defender:
                    shieldManager.DamageShield(damage, 0);
                    break;
                case CharacterType.Meditator:
                    DoDamage(shieldManager.DamageShield(damage, Health));
                    break;
            }
        }

        private void DoDamage(int damage = 1)
        {
            Health -= damage;
            ChangeHealthEvent.Invoke(Health);
        }

        private void CheckDeath(int health)
        {
            if (health <= 0)
            {
                Debug.Log("Game Over");
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


    }
}
