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
    public float softLengthAddition = 3f;
    public float softWidthAddition = 1f;

    public float softWindMultiplier = 0.2f;
    
    public float amountPerSquareMeter;
    
    public float wintensity;

    public float velocityLimit = 500f;

    private BoxCollider2D softcol;
    private BoxCollider2D hardcol;
    private ParticleSystem system;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 1f, 1f, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.right, Vector3.left * 2);
        Gizmos.DrawLine(Vector3.up, Vector3.right);
        Gizmos.DrawLine(Vector3.down, Vector3.right);
        Gizmos.DrawCube(Vector3.zero, new Vector3(length,width, 0f));
        
        Gizmos.color = new Color(0.3f, 1f, 1f, 0.3f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(length+softLengthAddition, width+softWidthAddition,0f));
        
        Setup();
    }

    private void Awake()
    {
        Setup();
    }


    public void Setup()
    {

        system = GetComponent<ParticleSystem>();
        AssignColliders();
        var shape = system.shape;
        shape.scale = new Vector3(shape.scale.x, width, length);

        var emission = system.emission;

        var amount = amountPerSquareMeter * width * length;

        emission.rateOverTime = amount;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        float softShellMultiplier = CalculateSoftShellMultiplier(other.transform.position, hardcol, softcol);
        
        if (!other.GetComponent<Rigidbody2D>()) return;
        var rb = other.GetComponent<Rigidbody2D>();
        if (rb.velocity.magnitude > velocityLimit && Vector2.Dot(rb.velocity,transform.right) > 0f) // Check if the vectors move in the same direction
        {
            return;
        }

        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        float sin = Mathf.Sin( angle );
        float cos = Mathf.Cos( angle );

        Vector2 forward = new Vector2(
            direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos
        );

        Vector2 addedVelocity;
        addedVelocity = wintensity * softShellMultiplier * Time.deltaTime * forward;

        rb.velocity += addedVelocity;
        if (rb.velocity.magnitude > velocityLimit)
        {
            rb.velocity = rb.velocity.normalized * velocityLimit;
        }
    }

    private void AssignColliders()
    {
        BoxCollider2D[] cols;

        cols = GetComponents<BoxCollider2D>();
        for (var i = 0; i < 2; i++)
        {
            if (cols[i] != null) continue;
            cols[i] = gameObject.AddComponent<BoxCollider2D>();
            cols[i].isTrigger = true;
        }
        hardcol = cols[0];
        softcol = cols[1];
        softcol.size = GetSoftSize();
        hardcol.size = GetHardSize();
    }
    
    private float CalculateSoftShellMultiplier(Vector2 collisionPosition, BoxCollider2D hardBox, BoxCollider2D softBox)
    {
        var actualCollisionPosition = new Vector3(collisionPosition.x,collisionPosition.y, transform.position.z);
        
        var multiplier = 0f;
        if (hardBox.bounds.Contains(actualCollisionPosition))
        {
            multiplier = 1f;
        }
        else if (softBox.bounds.Contains(actualCollisionPosition))
        {
            multiplier = softWindMultiplier;
        }
        
        Debug.Log("Wind Speed multiplier is : " + multiplier);
        return multiplier;
    }

    private Vector2 GetHardSize()
    {
        return new Vector2(length, width);
    }

    private Vector2 GetSoftSize()
    {
        return new Vector2(length+softLengthAddition, width+softWidthAddition);
    }
}
