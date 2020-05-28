using System.Collections;
using System.Collections.Generic;
using Spirinse.System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaceholderRestart : MonoBehaviour
{
    private Button button;
    public GameObject gameObjectToTurnOff;
    public void Awake() {
        button = GetComponent<Button>();
    }

    
    public void Update() {
        if (InputManager.Grabbing)
        {
            Restart();
        }
    }
    
    public void Restart()
    {
        gameObjectToTurnOff.SetActive(false);
        GameManager.Instance.Restart();
    }
}
