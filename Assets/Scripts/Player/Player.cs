using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spirinse.Player
{
    public enum CharacterType { Meditator, Defender }
    public class Player : MonoBehaviour
    {
        public PlayerAnimator playerAnimator;
        public Controls controls;
        public Meditator meditator;
        public Defender defender;

        public void OnEnable()
        {
            controls.playerMovementAction += playerAnimator.ChangeAnimation;
        }
    }
}
