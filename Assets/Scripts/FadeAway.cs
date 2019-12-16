using System.Collections;
using System.Collections.Generic;
using Spirinse.System;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 0f;
        //gameObject.SetActive(true);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (InputManager.Attacking)
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
