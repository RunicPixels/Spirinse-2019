using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ppCleanElephant : MonoBehaviour
{
    private PostProcessingManager ppManager;

    public BaseEnemy elephant;

    [Range(0, 1)] public float newProfileWeight;
    public float transitionSpeed = 2f;
    public bool newProfileUsed = false;

    private void Start()
    {
        elephant = GetComponent<BaseEnemy>();
        ppManager = PostProcessingManager.Instance;
        elephant.DieAction += SwitchOn;
    }

    public void Update()
    {
        if (newProfileUsed && newProfileWeight <= 1f) newProfileWeight += transitionSpeed * Time.deltaTime;
        else if (!newProfileUsed && newProfileWeight >= 0f) newProfileWeight -= transitionSpeed * Time.deltaTime;
        ppManager.rootCleanedVolume.weight = newProfileWeight;
    }

    public void SwitchOn()
    {
        newProfileUsed = true;
    }
}
