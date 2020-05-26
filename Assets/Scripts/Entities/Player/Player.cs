using System;
using System.Collections;
using System.Collections.Generic;
using Spirinse.Audio;
using UnityEngine;
using Spirinse.System.Player;

namespace Spirinse.Player
{
    public enum CharacterType
    {
        Meditator,
        Defender
    }

    public class Player : MonoBehaviour
    {
        public static Player Instance;
        public PlayerAnimator playerAnimator;
        public Controls controls; // Needs reorganizing.
        public Meditator meditator;
        public Defender defender;
        public SpiritPlayerSounds spiritPlayerSounds;



        public void OnEnable()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            controls.playerMovementAction += playerAnimator.ChangeAnimation;
            //SubscribeEvents();
        }

        public void OnDisable()
        {
            UnsubscribeEvents();
        }


        public float GetPlayerVelocity()
        {
            return controls.GetRB().velocity.magnitude;
        }

        private void SubscribeEvents()
        {
            //controls.dashAbility.OnAbilityUse += spiritPlayerSounds.DoDashSound;
        }

        private void UnsubscribeEvents()
        {
            //controls.dashAbility.OnAbilityUse -= spiritPlayerSounds.DoDashSound;
        }

        public void GotoSpawnPosition(Vector3 spawnPosition)
        {
            transform.position = spawnPosition;
            defender.transform.position = transform.position + new Vector3(0, 5, 0);
        }
    }
}
