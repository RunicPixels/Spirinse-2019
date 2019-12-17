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
        

        public static bool Grabbing  { private set; get; }
        public static bool Attacking { private set; get; }
        public static bool Dashing   { private set; get; }
        public static Vector2 Movement { private set; get; }

        public void Awake()
        {
            player = ReInput.players.GetPlayer(0);
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void SpiritUpdate()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            Grabbing = player.GetButton(RewiredConsts.Action.Grab);
            Attacking = player.GetButton(RewiredConsts.Action.Attack);
            Dashing = player.GetButton(RewiredConsts.Action.Dash);
            Movement = player.GetAxis2D(RewiredConsts.Action.Horizontal, RewiredConsts.Action.Vertical);
        }
    }
}
