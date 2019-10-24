using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Interfaces;
using System;

namespace Spirinse.Player
{
    public class Defender : MonoBehaviour, IDamagable
    {
        public Action<int, CharacterType> TakeDamageAction;
        public int TakeDamage(int damage)
        {
            TakeDamageAction.Invoke(damage, CharacterType.Defender);
            return 0;
        }

        // Start is called before the first frame update
        void Start()
        {

        }
    }
}
