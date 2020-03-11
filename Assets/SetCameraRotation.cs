using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraRotation : MonoBehaviour
{
    public float intensity;

    public Transform objectToLookAt;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(objectToLookAt);
        
        var targetRotation = Quaternion.LookRotation(objectToLookAt.transform.position - transform.position);
       
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, intensity* Time.deltaTime);
        
    }
}
