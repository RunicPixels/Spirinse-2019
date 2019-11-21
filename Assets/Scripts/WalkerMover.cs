using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkerMover : MonoBehaviour
{
    private Rigidbody2D rb;
    
    [Header("Tweak Those")]

    public float jumpSpeed = 30;
    public float moveAcceleration = 30;
    public float moveSpeed = 30;

    [Header("Dev Stuff(Can tweak but more specific)")]

    public float distToGround = 1;

    private float jump = 0;
    private float movementXY = 0;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        jump = (Input.GetButtonDown("Jump") && IsGrounded()) ? jumpSpeed + rb.velocity.y : 0f;

        movementXY = (Input.GetAxis("Horizontal") * moveAcceleration);
        
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(movementXY * Time.fixedDeltaTime, jump, 0));
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x,-moveSpeed, moveSpeed),rb.velocity.y,0);
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}
