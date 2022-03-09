using Rewired;
using Spirinse.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyContinue : MonoBehaviour
{
    private Button ContinueButton;

    public KeyCode ContinueKeyKeyboard;
    public KeyCode ContinueKeyXbox;

    void Awake()
    {
        ContinueButton = GetComponent<Button>();
    }

    void Update()
    {
        if (ReInput.players.GetPlayer(0).GetButton(RewiredConsts.Action.Grab)) {
            ContinueButton.onClick.Invoke();
        }
    }
}
