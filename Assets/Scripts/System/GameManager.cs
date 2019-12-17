using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spirinse.System.Health;
using Spirinse.Player;
using Spirinse.System.UI;
using Spirinse.System.Effects;
using UnityEngine.SceneManagement;

namespace Spirinse.System
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [field: SerializeField] public HealthManager HealthManager { get; protected set; }
        [field: SerializeField] public PlayerManager PlayerManager { get; protected set; }
        [field: SerializeField] public UIManager UiManager { get; protected set; }
        [field: SerializeField] public InputManager InputManager { get; protected set; }
        [field: SerializeField] public GameStateManager StateManager { get; protected set; }
        [field: SerializeField] public CleanseManager CleanseManager { get; protected set; }
        [field: SerializeField] public EffectsManager EffectsManager { get; protected set; }

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
            else                        Destroy(gameObject);

            if (HealthManager == null)  HealthManager   = HealthManager.Instance;
            if (InputManager == null)   InputManager    = InputManager.Instance;
            if (PlayerManager == null)  PlayerManager   = PlayerManager.Instance;
            if (UiManager == null)      UiManager       = UIManager.Instance;
            if (StateManager == null)   StateManager    = GameStateManager.Instance;
            if (CleanseManager == null) CleanseManager  = CleanseManager.Instance;
            if (EffectsManager == null) EffectsManager  = EffectsManager.Instance;
        }

        private void SetupEvents()
        {
            // Manage Health Events
            UiManager.GetHealthUI.SetMaxHealthContainers(HealthManager.GetHealthCap);

            HealthManager.ChangeHealthEvent            += UiManager.GetHealthUI.ChangeCurrentHealth;
            HealthManager.ChangeHealthEvent            += StateManager.CheckGameOver;
            HealthManager.ChangeMaxHealthEvent         += UiManager.GetHealthUI.ChangeMaxHealth;
        
            // Manage Player Events
            var meditator                               = PlayerManager.GetPlayer().meditator;
            var defender                                = PlayerManager.GetPlayer().defender;

            meditator.TakeDamageAction                 += HealthManager.HitMeditator;
            meditator.TakeDamageAction                 += EffectsManager.timeManager.PlayerHitFreeze;
            meditator.TakeDamageAction                 += EffectsManager.cameraShake.PlayerHitShake;
           
            defender.TakeDamageAction                  += HealthManager.HitDefender;
            defender.TakeDamageAction                  += EffectsManager.timeManager.PlayerHitFreeze;
            defender.TakeDamageAction                  += EffectsManager.cameraShake.PlayerHitShake;

            // Manage Player Action Events
            defender.tempControls.useDashAction        += HealthManager.SetIFramesCD;

            // Game Over Events
            StateManager.GameOverEvent                 += InitGame;
            
            // ...
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            CameraManager.Instance.followPlayer.FindPlayer();
            InitGame();
        }
        
        private void InitGame()
        {
            HealthManager.InitHealth();
        }

        public void Update()
        {
            InputManager.SpiritUpdate();
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