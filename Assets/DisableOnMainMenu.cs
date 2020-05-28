using System.Collections;
using System.Collections.Generic;
using Spirinse.System;
using UnityEngine;
using UnityEngine.UI;

public class DisableOnMainMenu : MonoBehaviour
{
    public GameObject[] partsToDisable;
    // Start is called before the first frame update
    private void OnEnable()
    {
        GameManager.Instance.GameStateChangeEvent += DoStateChange;
    }

    private void OnDisable()
    {
        if (GameManager.Instance.GameStateChangeEvent != null)
            GameManager.Instance.GameStateChangeEvent -= DoStateChange;
    }


    private void DoStateChange(GameManager.GameState state)
    {
        var isActive = state == GameManager.GameState.Playing;

        foreach (GameObject gameObj in partsToDisable)
        {
            gameObj.SetActive(isActive);
        }
    }
}
