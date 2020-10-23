using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsInfluencedObject : MonoBehaviour
{
    private Rigidbody2D rb;

    private float gravityValue;

    public float reducedGravityValue;

    private bool isGravityReduced = false;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityValue = rb.gravityScale;
    }

    private void ReduceGravity()
    {
        if (!isGravityReduced)
        {
            gravityValue = rb.gravityScale;
        }
        rb.gravityScale = reducedGravityValue;
        if (isGravityReduced) return;
        isGravityReduced = true;
    }

    private void ReturnGravity()
    {
        if (!isGravityReduced) return;
        rb.gravityScale = gravityValue;
        isGravityReduced = false;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag(Statics.TagWind)) ReduceGravity();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(Statics.TagWind)) ReduceGravity();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag(Statics.TagWind)) ReturnGravity();
    }

}
