using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Spirinse.Interfaces;
using Spirinse.System.Combat;
using UnityEngine;

namespace Spirinse.Abilities
{
    public class BaseAttack : BaseAbility, IAttack
    {
        public int damage = 6;
        public float energySteal = 0f;

        private DamageType damageType;

        public void Awake()
        {
            damageType = SetDamageType();
        }

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

        private DamageType SetDamageType()
        {
            var dt = GetComponent<DamageType>();
            if (dt == null)
            {
                dt = gameObject.AddComponent<DamageType>();
                dt.damageType = System.Enums.DamageEnums.DamageType.BasicAttack;
            }
            return dt;
        }
    }
}