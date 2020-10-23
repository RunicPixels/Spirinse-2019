using System.Collections;
using System.Collections.Generic;
using Rewired;
using Spirinse.System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public int levelToGoTo;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(Statics.TagPlayer) == true)
        {
            GameManager.Instance.LoadLevel(levelToGoTo);
        }
    }
}
