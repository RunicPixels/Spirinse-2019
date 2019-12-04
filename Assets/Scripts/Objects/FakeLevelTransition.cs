using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Interfaces;
using System;

public class FakeLevelTransition : MonoBehaviour
{
    public GameObject barrier;
    public GameObject enemyGroup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
          if (barrier.activeSelf)
          {
            barrier.SetActive(false);
            enemyGroup.SetActive(true);
          }



          Debug.Log("Switch!");

        }

    }
}
