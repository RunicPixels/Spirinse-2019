using Spirinse.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spirinse.Player {
    public class Meditator : MonoBehaviour, IDamagable
    {
        public static Meditator Instance;

        public Action<int> TakeDamageAction;

        public LayerMask hitLayers;

        public float iFrames = 1f;

        private float iFramesCD;
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

        public void TakeDamage(int damage)
        {
            // Loop Abilities here for damage reduction
            TakeDamageAction?.Invoke(damage);
        }
    }
}