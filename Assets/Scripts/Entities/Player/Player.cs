﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.System.Player;

namespace Spirinse.Player
{
    public enum CharacterType { Meditator, Defender }
    public class Player : MonoBehaviour
    {
        public static Player Instance;
        public PlayerAnimator playerAnimator;
        public Controls controls; // Needs reorganizing.
        public Meditator meditator;
        public Defender defender;
        public CheckPointManager checkPointManager;


        public void OnEnable()
        {
            if (checkPointManager == null) checkPointManager = new CheckPointManager();

            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            controls.playerMovementAction += playerAnimator.ChangeAnimation;
        }
    }
}
