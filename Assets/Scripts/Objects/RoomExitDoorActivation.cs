using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Interfaces;
using System;

public class RoomExitDoorActivation : MonoBehaviour
{
    public GameObject exitDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            exitDoor.SetActive(false);
        }

    }
}
