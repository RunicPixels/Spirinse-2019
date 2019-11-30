using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Interfaces;
using System;

public class FakeLevelTransition1 : MonoBehaviour
{
    public GameObject levelZero;
    public GameObject levelOne;
    public GameObject levelTwo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
          if (levelOne.activeSelf)
          {
            levelOne.SetActive(false);
            levelTwo.SetActive(true);
          }
          Debug.Log("Switch!");

        }

    }
}
