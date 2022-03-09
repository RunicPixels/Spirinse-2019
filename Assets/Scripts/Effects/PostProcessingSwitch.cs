using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class PostProcessingSwitch : MonoBehaviour
{
    private PostProcessingManager ppManager;
    [Range(0,1)] public float newProfileWeight;
    public float transitionSpeed = 1f;
    public bool newProfileUsed = false;
    private float lastTouchedTime =-1f;

    private void Start()
    {
        ppManager = PostProcessingManager.Instance;
    }

    public void Update()
    {
        // using cave for now.
        if (lastTouchedTime > 0f)
        {
            if (newProfileUsed && newProfileWeight <= 1f) newProfileWeight += transitionSpeed * Time.deltaTime;
            else if (!newProfileUsed && newProfileWeight >= 0f) newProfileWeight -= transitionSpeed * Time.deltaTime;
            lastTouchedTime -= Time.deltaTime;
        }
        ppManager.caveVolume.weight = newProfileWeight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag(Statics.TagPlayer))
        {
            newProfileUsed = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(Statics.TagPlayer))
        {
            lastTouchedTime = transitionSpeed + 0.1f;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Statics.TagPlayer))
        {
            newProfileUsed = false;
        }
    }
}


