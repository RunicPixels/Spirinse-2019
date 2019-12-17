using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Player;
using UnityEngine.SceneManagement;

namespace Spirinse.System
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        private Player.Player player;
        public void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(Instance);
            if (player == null)
            {
                Debug.LogWarning("No Player Assigned to Player Manager!!!!");
                player = GetPlayer();
            }
        }

        public Player.Player GetPlayer()
        {
            if (player) return player;
            player = FindObjectOfType<Player.Player>();

            return player;
        }
        
    }
}
