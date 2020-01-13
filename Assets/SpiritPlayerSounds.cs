using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Spirinse.Player;
using Spirinse.System;

namespace Spirinse.Audio
{
    public class SpiritPlayerSounds : MonoBehaviour
    {
        Player.Player player;

        [FMODUnity.EventRef]
        public string dashSound;

        private void Start()
        {
            player.controls.dashAbility.OnAbilityUse += DoDashSound;
        }

        void DoDashSound()
        {
            RuntimeManager.PlayOneShot(dashSound);
        }
    }
}
