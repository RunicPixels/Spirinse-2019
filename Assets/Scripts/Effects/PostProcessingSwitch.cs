using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class PostProcessingSwitch : MonoBehaviour
{
    private GameObject ppManager;
    public PostProcessProfile otherProfile;
    private PostProcessProfile oldProfile;

    private void Start()
    {
        ppManager = PostProcessingManager.Instance.gameObject;
        oldProfile = ppManager.GetComponent<PostProcessVolume>().profile;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Defender"))
        {
            ppManager.GetComponent<PostProcessVolume>().profile = otherProfile;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Defender"))
        {
            ppManager.GetComponent<PostProcessVolume>().profile = oldProfile;
        }
    }
}


