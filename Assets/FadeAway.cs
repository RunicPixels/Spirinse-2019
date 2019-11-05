using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.anyKey)
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}
