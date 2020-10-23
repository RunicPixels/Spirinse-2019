using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Objects;
using Spirinse.Interfaces;
using Rewired;
using UnityEngine.Events;
using InputManager = Spirinse.System.InputManager;

public class Grab : BaseAbility
{
    public bool grabbing = false;

    private IGrabbable heldObject;

    public Rigidbody2D parentRB;

    public float launchVelocity;
    public float grabRadius = 3f;
    public CircleCollider2D col;
    public GameObject sphere;
    public Joint2D joint;

    public float chiDrain;

    public Action<Transform, IGrabbable> OnGrab;
    public Action<Transform, IGrabbable> OnRelease;

    private void Update()
    {
        transform.localPosition = Vector3.zero;
        grabbing = InputManager.Grabbing;

        if(grabbing)
        {
            Controls.chi -= chiDrain * Time.deltaTime;
            Controls.chi = Mathf.Max(0f, Controls.chi);
        }
    }

    private void FixedUpdate()
    {
        if (grabbing)
        {
            transform.localPosition = Vector3.zero;
            sphere.SetActive(true);
            col.enabled = true;
            if (col.radius < grabRadius) col.radius += 12 * Time.fixedDeltaTime;
            var radius = col.radius;
            sphere.transform.localScale = new Vector3(radius * 2f, radius *2f, radius);
        }
        else
        {
            col.enabled = false;
            col.radius = 0f;
            sphere.SetActive(false);
            if (heldObject != null) Launch(launchVelocity);
            
            
        }

        if (heldObject != null)
        {
            sphere.SetActive(false);
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
        joint.connectedBody = heldObject.GetRigidbody2D();
        heldObject.Hold(transform);
        OnGrab?.Invoke(transform,heldObject);
    }

    private void Launch(float velocity = 0f)
    {
        OnRelease?.Invoke(transform,heldObject);
        heldObject.Release(parentRB.velocity + parentRB.velocity.normalized * velocity);
        joint.connectedBody = null;
        heldObject = null;
    }
}
