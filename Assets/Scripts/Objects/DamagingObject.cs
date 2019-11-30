using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Interfaces;
using System;

public class DamagingObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Throwable")
        {
          Debug.Log("Au!");
          Destroy(this.gameObject);
        }

    }
}
