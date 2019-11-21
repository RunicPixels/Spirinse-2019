using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Objects;
using Spirinse.Interfaces;

public class Grab : BaseAbility
{
    public bool grabbing = false;

    private IGrabbable heldObject;

    public Rigidbody2D parentRB;

    public CircleCollider2D col;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space)) {
            grabbing = true;
        }
        else if (grabbing)
        {
            grabbing = false;
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
            if (heldObject != null) Launch();
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

    private void Launch()
    {
        heldObject.Release(parentRB.velocity);
        heldObject = null;
    }
}
