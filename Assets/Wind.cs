﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [Header("Wind Direction without any rotation")]
    public Vector3 direction     = Vector3.right;

    public float width = 3;
    public float length = 3;

    public float amountPerSquareMeter;
    
    public float wintensity;

    private BoxCollider2D col;
    private ParticleSystem system;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 1f, 1f, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, new Vector3(length,width, 0f));
    }

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        system = GetComponent<ParticleSystem>();
        
        col.size = new Vector2(length,width);
        var shape = system.shape;
        shape.scale = new Vector3(shape.scale.x, width, length);

        var emission = system.emission;

        var amount = amountPerSquareMeter * width * length;

        emission.rateOverTime = amount;
    }
    
    #if
    
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
        rb.velocity *= 0.7f;

        rb.velocity += wintensity * Time.deltaTime * forward;
    }
}
