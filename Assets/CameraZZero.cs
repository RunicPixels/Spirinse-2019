using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZZero : MonoBehaviour
{
    public Transform objectToFollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var position = objectToFollow.transform.position;
        transform.position = new Vector3(position.x, position.y, 0f);
    }
}
