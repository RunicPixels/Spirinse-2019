using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [Header("Wind Direction without any rotation")]
    public Vector3 direction     = Vector3.right;
    
    public float wintensity;
    public void OnTriggerStay2D(Collider2D other)
    {
        if (!other.GetComponent<Rigidbody2D>()) return;
        var rb = other.GetComponent<Rigidbody2D>();
        
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        float sin = Mathf.Sin( angle );
        float cos = Mathf.Cos( angle );

        Vector2 forward = new Vector2(
            direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos
        );

        rb.velocity += wintensity * Time.deltaTime * forward;
    }
}
