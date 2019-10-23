using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace Spirinse.System
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        private Action OnAttack;
        private Action OnDodge;

        public void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
    }
}
