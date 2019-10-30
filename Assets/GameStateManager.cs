using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spirinse.System
{
    public class GameStateManager : MonoBehaviour
    {
        private static GameStateManager Instance;

        public Action GameOverEvent;

        // Start is called before the first frame update
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            GameOverEvent += DebugGameOver;
        }

        public void CheckGameOver(int health)
        {
            if (health > 0) return;
            GameOverEvent.Invoke();
        }

        public void DebugGameOver()
        {
            Debug.Log("Game Over");
        }
    }
}
