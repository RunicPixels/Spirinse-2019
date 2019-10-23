using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Player;

namespace Spirinse.System
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;
        public Player.Player player;
        public void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(Instance);
            if (player == null) Debug.LogWarning("No Player Assigned to Player Manager!!!!");
        }
    }
}
