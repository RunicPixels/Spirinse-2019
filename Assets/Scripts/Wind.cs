using System;
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
    [Range(0f, 1f)] public float drag = 0.3f;

    private BoxCollider2D col;
    private ParticleSystem system;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 1f, 1f, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.right, Vector3.left * 2);
        Gizmos.DrawLine(Vector3.up, Vector3.right);
        Gizmos.DrawLine(Vector3.down, Vector3.right);
        Gizmos.DrawCube(Vector3.zero, new Vector3(length,width, 0f));
        Setup();
    }

    private void Awake()
    {
        Setup();
    }


    public void Setup()
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

        rb.velocity *= 1-drag;

        rb.velocity += wintensity * Time.deltaTime * forward;
    }
}
