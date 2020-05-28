using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Player;
using Spirinse.System.Player;
using UnityEngine.SceneManagement;

namespace Spirinse.System.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;

        private Spirinse.Player.Player player;

        public CheckPointManager checkPointManager;

        public void Awake()
        {
            if (checkPointManager == null) Debug.LogWarning("Please asign a checkpoint manager");
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            if (player == null)
            {
                //Debug.LogWarning("No Player Assigned to Player Manager!");
                player = GetPlayer();
            }
        }

        public Spirinse.Player.Player GetPlayer()
        {
            if (player) return player;
            player = FindObjectOfType<Spirinse.Player.Player>();

            return player;
        }
        public void OnInit()
        {
            checkPointManager.SetNewCheckPoint(player.defender.transform.position);
        }
        public void OnRestart()
        {
            Debug.Log("On Restart");
            player.defender.transform.position = checkPointManager.GetCurrentCheckPoint();
        }
    }
}
