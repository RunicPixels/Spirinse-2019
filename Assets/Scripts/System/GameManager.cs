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

        [SerializeField] protected HealthManager healthManager;
        public HealthManager HealthManager => healthManager;

        [SerializeField] protected PlayerManager playerManager;
        public PlayerManager PlayerManager => playerManager;

        protected UIManager uIManager;
        protected InputManager inputManager;

        // Start is called before the first frame update
        private void Start()
        {
            SetupConnections();
            InitGame();
        }

        private void SetupConnections()
        {
            // Assign Managers & Variables
            if (Instance == null)       Instance        = this;
            else Destroy(gameObject);

            if (healthManager == null)  healthManager   = HealthManager.Instance;
            if (inputManager == null)   inputManager    = InputManager.Instance;
            if (playerManager == null)  playerManager   = PlayerManager.Instance;
            if (uIManager == null)      uIManager       = UIManager.Instance;

            // Manage Health Events
            var shieldManager = healthManager.ShieldManager;

            uIManager.GetHealthUI.SetMaxHealthContainers(HealthManager.GetHealthCap);
            uIManager.GetShieldUI.SetMaxShieldContainers(shieldManager.ShieldCap);

            healthManager.ChangeHealthEvent            += uIManager.GetHealthUI.ChangeCurrentHealth;
            healthManager.ChangeMaxHealthEvent         += uIManager.GetHealthUI.ChangeMaxHealth;

            shieldManager.ChangeShieldEvent            += uIManager.GetShieldUI.ChangeCurrentShield;
            shieldManager.ChangeMaxShieldEvent         += uIManager.GetShieldUI.ChangeMaxShield;
        
            // Manage Player Events
            var meditator = playerManager.player.meditator;
            var defender = playerManager.player.defender;

            meditator.TakeDamageAction                 += healthManager.Hit;
            defender.TakeDamageAction                  += healthManager.Hit;

            // ...
        }
        private void InitGame()
        {
            healthManager.InitHealth();
            healthManager.InitShield();
        }
    }
}
