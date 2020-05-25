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
        if (Input.GetKeyDown(ContinueKeyKeyboard))
        {
            ContinueButton.onClick.Invoke();
        }

        if (Input.GetKeyDown(ContinueKeyXbox))
        {
            ContinueButton.onClick.Invoke();
        }
    }
}
