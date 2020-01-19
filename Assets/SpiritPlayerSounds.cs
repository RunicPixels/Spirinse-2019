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

        public void DoDashSound()
        {
            RuntimeManager.PlayOneShot(dashSound);
        }
    }
}
