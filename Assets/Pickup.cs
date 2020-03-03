using System.Collections;
using System.Collections.Generic;
using Spirinse.Player;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Defender") || col.CompareTag("Spirit"))
        {
            DoPickup();
        }
    }

    private void DoPickup()
    {
        Player.Instance.controls.canAttack = true;
        Destroy(gameObject);
    }
}
