using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Objects;
using Spirinse.Interfaces;
using Rewired;

public class Grab : BaseAbility
{
    public bool grabbing = false;

    private IGrabbable heldObject;

    public Rigidbody2D parentRB;

    public float launchVelocity;

    public CircleCollider2D col;

    public float chiDrain;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.Q) == false) {
            grabbing = true;
        }

        else if(Input.GetKeyDown(KeyCode.Q) && grabbing)
        {
            grabbing = false;
            Launch();
        }

        else if (grabbing)
        {
            grabbing = false;
        }

        if(grabbing)
        {
            Controls.chi -= chiDrain * Time.deltaTime;
            Controls.chi = Mathf.Max(0f, Controls.chi);
        }
    }

    private void FixedUpdate()
    {
        if(grabbing)
        {
            col.enabled = true;
            if(col.radius < 3f) col.radius += 24 * Time.fixedDeltaTime;


        }
        else
        {
            col.enabled = false;
            col.radius = 0f;
            if (heldObject != null) Launch(launchVelocity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(heldObject == null && collision.GetComponent<BaseGrabObject>())
        {
            Attach(collision.GetComponent<BaseGrabObject>());
        }
    }
    private void Attach(IGrabbable grab)
    {
        heldObject = grab;
        heldObject.Hold(transform);
    }

    private void Launch(float velocity = 0f)
    {
        heldObject.Release(parentRB.velocity + parentRB.velocity.normalized * velocity);
        heldObject = null;
    }
}
