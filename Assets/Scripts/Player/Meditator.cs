using Spirinse.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spirinse.Player {
    public class Meditator : MonoBehaviour, IDamagable
    {
        public static Meditator Instance;

        public Action<int, CharacterType> TakeDamageAction;

        public LayerMask hitLayers;

        public float iFrames = 1f;

        private float iFramesCD;
        // Start is called before the first frame update
        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(Instance);
                Instance = this;
            }
        }

        private void Update()
        {
            if (iFramesCD > 0f) iFramesCD -= Time.deltaTime;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (iFramesCD > 0f) return;
            var enemy = other.GetComponent<PrototypeEnemy>();
            if (enemy != null)
            {
                // Note to future : For now much of this code is pretty hard coded, variables might get introduced if there's a need for more leeway in taking damage.
                enemy.TakeDamage(0);
                Grow.currentGrowth -= 1f;
                iFramesCD = 1f;
                // Note to future : for now locked at one damage, add damage variable to enemies and change this in order to add more damage.
                TakeDamage(1);
            }
        }
        public int TakeDamage(int damage)
        {
            TakeDamageAction.Invoke(damage,CharacterType.Meditator);
            return damage;
        }
    }
}