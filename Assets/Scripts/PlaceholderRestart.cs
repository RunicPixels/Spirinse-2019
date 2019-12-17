using System.Collections;
using System.Collections.Generic;
using Spirinse.System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceholderRestart : MonoBehaviour
{
    public void Restart()
    {
        GameManager.Instance.Restart();
    }
}
