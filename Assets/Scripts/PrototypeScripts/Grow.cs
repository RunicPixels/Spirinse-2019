using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour
{
    public float growthScale;

    public static float currentGrowth = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentGrowth += growthScale * Time.deltaTime;
        transform.localScale = Vector3.one * currentGrowth;
        if (currentGrowth < 0) currentGrowth = 0f;
    }
}
