using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnTrigger2D : MonoBehaviour
{
    public GameObject objectToEnable;
    private bool spawned = false;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Defender") && spawned == false)
        {
            objectToEnable.SetActive(true);
            spawned = true;
        }
    }
}
