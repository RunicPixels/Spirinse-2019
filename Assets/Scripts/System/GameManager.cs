using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spirinse.System.Health;
using Spirinse.Player;
using Spirinse.System.UI;

namespace Spirinse.System
{
    public class GameManager : MonoBehaviour
    {
        // Future Edit Notes: Input needs to be done from Input Manager

        private static GameManager Instance;

        [field: SerializeField]
        public HealthManager HealthManager { get; protected set; }

        [field: SerializeField]
        public PlayerManager PlayerManager { get; protected set; }

        [field: SerializeField]
        public UIManager UiManager { get; protected set; }

        [field: SerializeField]
        public InputManager InputManager { get; protected set; }

        [field: SerializeField]
        public GameStateManager StateManager { get; protected set; }

        // Start is called before the first frame update
        private void Start()
        {
            SetupConnections();
            SetupEvents();
            InitGame();
        }

        private void SetupConnections()
        {
            // Assign Managers & Variables
            if (Instance == null)       Instance        = this;
            else Destroy(gameObject);

            if (HealthManager == null)  HealthManager   = HealthManager.Instance;
            if (InputManager == null)   InputManager    = InputManager.Instance;
            if (PlayerManager == null)  PlayerManager   = PlayerManager.Instance;
            if (UiManager == null)      UiManager       = UIManager.Instance;
            if (StateManager == null)   StateManager    = GameStateManager.Instance;
        }

        private void SetupEvents()
        {
            // Manage Health Events
            var shieldManager = HealthManager.ShieldManager;

            UiManager.GetHealthUI.SetMaxHealthContainers(HealthManager.GetHealthCap);
            UiManager.GetShieldUI.SetMaxShieldContainers(shieldManager.ShieldCap);

            HealthManager.ChangeHealthEvent            += UiManager.GetHealthUI.ChangeCurrentHealth;
            HealthManager.ChangeHealthEvent            += StateManager.CheckGameOver;
            HealthManager.ChangeMaxHealthEvent         += UiManager.GetHealthUI.ChangeMaxHealth;

            shieldManager.ChangeShieldEvent            += UiManager.GetShieldUI.ChangeCurrentShield;
            shieldManager.ChangeMaxShieldEvent         += UiManager.GetShieldUI.ChangeMaxShield;
        
            // Manage Player Events
            var meditator = PlayerManager.player.meditator;
            var defender = PlayerManager.player.defender;

            meditator.TakeDamageAction                 += HealthManager.HitMeditator;
            defender.TakeDamageAction                  += HealthManager.HitDefender;

            // Game Over Events
            StateManager.GameOverEvent                 += InitGame;

            // ...
        }

        private void InitGame()
        {
            HealthManager.InitHealth();
            HealthManager.InitShield();
        }
    }
}



//public class FMODImplementation
//{
//    //HIERIN KOMT FMOD Implemetatie voor audio.
//    public void PlayOnce(params)
//    {

//    }
//}

//public class AudioManager
//{
//    public PlayerAudio playerAudio = new PlayerAudio();
//    public EnvironmentAudio environmentAudio;
//}

//public class PlayerAudio
//{
//    private FMODImplementation fmodImplementation;
//    public void PlayHitSound(int damage, CharacterType type)
//    {
//        fmodImplementation.PlayOnce(params);
//    }
//}

//public class EnvironmentAudio
//{
//    public void PlayBirdSound()
//    {

//    }
//}