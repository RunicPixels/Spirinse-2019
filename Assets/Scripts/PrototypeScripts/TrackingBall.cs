using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBall : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform positionOverride;
    public float maxDistance;
    public float intensity;
    public float horizontalMultiplier;
    public float verticalMultiplier;

    private bool doPositionOverride = false;
    // Start is called before the first frame update
    void Start()
    {
        if (positionOverride != null) doPositionOverride = true;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Vector3.zero;
        if (doPositionOverride)
        {
            pos = positionOverride.position;
        }
        else
        {
            pos = rb.transform.position;
        }

        transform.position = pos + Vector3.Scale(Vector3.ClampMagnitude(rb.velocity * intensity , maxDistance) - (Vector3.forward * 10),new Vector3(horizontalMultiplier,verticalMultiplier));
    }
}
