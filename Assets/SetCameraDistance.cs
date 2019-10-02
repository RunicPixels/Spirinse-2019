using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraDistance : MonoBehaviour
{
    private float baseDistance = 4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        var distance = Mathf.Max(18f,baseDistance + Grow.currentGrowth);
        position = new Vector3(position.x,position.y,-distance);
        transform.position = position;
    }
}
