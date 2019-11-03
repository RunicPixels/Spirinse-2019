using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spirinse.System.Health
{
    [SerializeField]
    public class ShieldManager : MonoBehaviour
    {
        // Future Edit Notes : Maybe Inherit from HealthManager?

        [SerializeField] protected int shield;
        [SerializeField] protected int maxShield;
        [SerializeField] protected int shieldCap;
        public int ShieldCap => shieldCap;

        public Action<int> ChangeShieldEvent;
        public Action<int> ChangeMaxShieldEvent;

        public void Awake()
        {
            ChangeShieldEvent += PlaceHolderFunction;
            ChangeMaxShieldEvent += PlaceHolderFunction;
            InitShield();
        }

        // Will deal damage by the given value until the damage cap is reached.
        public int DamageShield(int damage, int damageCap = 0)
        {
            if (shield < damageCap) return damage;

            var damageToDeal = Mathf.Min(damage, shield - damageCap + 1);

            SetShield(shield - damageToDeal);
            SetNewMaxShield(maxShield);

            return damage - damageToDeal;

        }

        public void RestoreShield(int amount)
        {
            shield += amount;
            if (shield > maxShield) shield = maxShield;
            ChangeShieldEvent?.Invoke(shield);
        }

        public void IncreaseShield(int amount)
        {
            maxShield += amount;
            if (maxShield > shieldCap) maxShield = shieldCap;
            ChangeMaxShieldEvent?.Invoke(maxShield);
        }
        public void SetShield(int amount)
        {
            shield = amount;
            ChangeShieldEvent?.Invoke(shield);
        }

        public void SetNewMaxShield(int amount)
        {
            maxShield = amount;
            ChangeMaxShieldEvent?.Invoke(maxShield);
        }

        public void SetShieldCap(int amount)
        {
            shieldCap = amount;
            ChangeMaxShieldEvent?.Invoke(maxShield);
        }
        [ClickableFunction]
        public void InitShield()
        {
            UI.UIManager.Instance.GetShieldUI.SetMaxShieldContainers(ShieldCap);
            ChangeShieldEvent?.Invoke(shield);
            ChangeMaxShieldEvent?.Invoke(maxShield);
        }

        public void PlaceHolderFunction(int amount)
        {
            // Yoink!
        }
    }
}
