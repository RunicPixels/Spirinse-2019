using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantCutscene : MonoBehaviour
{
    [SerializeField] private Animator myAnimationController;

    public Canvas TextCanvas;
    public Transform Dialogue;


    private void Start()
    {
        TextCanvas.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Statics.TagPlayer))
        { 
            myAnimationController.SetBool("EnterCutscene", true);
            Dialogue.gameObject.SetActive(true);
            TextCanvas.enabled = true;
        }
    }
}
