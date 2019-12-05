using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Interfaces;
using System;

public class CleanseCorruption : MonoBehaviour
{
    public GameObject activateOnMeditate;
    public GameObject deactivateOnMeditate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            activateOnMeditate.SetActive(true);
            deactivateOnMeditate.SetActive(false);


            Debug.Log("Switch!");

        }

    }
}
