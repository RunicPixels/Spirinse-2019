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

        public int TakeDamage(int damage)
        {
            TakeDamageAction.Invoke(damage,CharacterType.Meditator);
            return damage;
        }
    }
}