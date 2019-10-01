using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meditator : MonoBehaviour
{
    public static Meditator Instance;


    // Start is called before the first frame update
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }
}
