using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using FMODUnity;
using Spirinse.Player;
using Spirinse.System;
using Spirinse.System.Player;

namespace Spirinse.Audio
{
    public class SpiritPlayerSounds : MonoBehaviour
    {
        [SerializeField] protected AbilitySound[] AbilitySounds;
        [Serializable] public class AbilitySound
        {
            private Rigidbody2D cachedRB;
            private Player.Player player;
            public BaseAbility ability;
            [FMODUnity.EventRef] public string AbilityUseEvent;

            private FMOD.Studio.EventInstance _eventInstance;

            public void Update()
            {
                _eventInstance.set3DAttributes(
                    FMODUnity.RuntimeUtils.To3DAttributes(
                        player.defender.transform.gameObject,
                        cachedRB));
            }
            
            public void Begin()
            {
                _eventInstance = FMODUnity.RuntimeManager.CreateInstance(AbilityUseEvent);
                ability.OnAbilityUse += PlaySound;
                cachedRB = Player.Player.Instance.controls.GetRB();
                player = Player.Player.Instance;

            }
            
            public void PlaySound()
            {

                
                _eventInstance.start();
                //FMODUnity.RuntimeManager.PlayOneShot(AbilityUseEvent,Player.Player.Instance.defender.transform.position);
            }
        }
        
        private void Start()
        {
            for (int i = 0; i < AbilitySounds.Length; i++)
            {
                AbilitySounds[i].Begin();
            }
        }

        private void Update()
        {
            for (int i = 0; i < AbilitySounds.Length; i++)
            {
                AbilitySounds[i].Update();
            }
        }
    }
}