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

        private float iFrames = 0.5f;
        private float iFramesCalc;

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

        private void OnEnable()
        {
            ChangeHealthEvent += CheckDeath;
            ChangeMaxHealthEvent += CheckDeath;

            Health = MaxHealth;
            instance = this;

            InitShield();
        }

        private void OnDisable()
        {
            ChangeHealthEvent -= CheckDeath;
            ChangeMaxHealthEvent -= CheckDeath;
        }

        private void Update()
        {
            if (iFramesCalc > 0)
            {
                iFramesCalc -= Time.deltaTime;
                // Create iFrame visuals;
            }
        }

        [ClickableFunction]
        public void InitHealth()
        {
            Health = MaxHealth;
            UI.UIManager.Instance.GetHealthUI.SetMaxHealthContainers(healthCap);
            ChangeHealthEvent?.Invoke(Health);
            ChangeMaxHealthEvent?.Invoke(MaxHealth);
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

        public void HitMeditator(int damage)
        {
            if (iFramesCalc > 0) return;
            
            DoDamage(shieldManager.DamageShield(damage, Health));

            SetIFramesCD(iFrames);

        }

        public void HitDefender(int damage)
        {
            if (iFramesCalc > 0) return;

            SetIFramesCD(iFrames);

            shieldManager.DamageShield(damage, 0);
        }

        public void SetIFramesCD(float amount)
        {
            iFramesCalc = amount;
        }

        private void DoDamage(int damage = 1)
        {
            Health -= damage;
            ChangeHealthEvent?.Invoke(Health);
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
