using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spirinse.System
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; } = null;

        public Action GameOverEvent;

        public Text temporaryTextGameOverWin;
        public Transform temporaryTextAndButtonContainer;


        // Start is called before the first frame update
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            GameOverEvent += DebugGameOver;
            GameOverEvent += DoGameOver;
        }

        public void CheckGameOver(int health)
        {
            if (health > 0) return;
            GameOverEvent?.Invoke();
        }

        public void DebugGameOver()
        {
            //Debug.Log("Game Over");
        }

        public void DoGameOver()
        {
            temporaryTextGameOverWin.text = "You <b><color=red>lost</color></b>... :C";
            temporaryTextAndButtonContainer.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
