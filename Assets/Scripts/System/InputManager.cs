using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

namespace Spirinse.System
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        private Action OnAttack;
        private Action OnDodge;
        private Rewired.Player player;

        private const string Grab = "Grab";

        public bool Grabbing { private set; get; }

        public void Awake()
        {
            player = ReInput.players.GetPlayer(0);
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

        }

        public void SpiritUpdate()
        {
            if (player.GetButton(Grab))
            {
                Grabbing = true;
            }
            else
            {
                Grabbing = false;
            }
        }
    }
}
