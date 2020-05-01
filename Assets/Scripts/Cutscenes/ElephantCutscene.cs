using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantCutscene : MonoBehaviour
{
    [SerializeField] private Animator myAnimationController;
    public Canvas TextCanvas;


    private void Start()
    {
        TextCanvas.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Defender"))
        { 
            myAnimationController.SetBool("EnterCutscene", true);
            TextCanvas.enabled = true;
        }
    }
}
