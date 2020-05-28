using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameras : MonoBehaviour
{
    public static  CinemachineCameras Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
