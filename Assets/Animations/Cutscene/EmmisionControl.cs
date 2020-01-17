using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmisionControl : MonoBehaviour {

    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        material.EnableKeyword ("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
