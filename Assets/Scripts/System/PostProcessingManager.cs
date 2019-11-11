using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingManager : MonoBehaviour
{
    public static PostProcessingManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}
