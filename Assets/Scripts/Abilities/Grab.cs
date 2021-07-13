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
    public ParticleSystem particles;

    public float chiDrain;

    public Action<Transform, IGrabbable> OnGrab;
    public Action<Transform, IGrabbable> OnRelease;

    private float particleTimeRate;
    private float particleDistanceRate;

    private void Start()
    {
        particleTimeRate = particles.emission.rateOverTime.constant;
        particleDistanceRate = particles.emission.rateOverDistance.constant;
    }
    
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
        var radius = col.radius;
        if (grabbing & heldObject == null)
        {
            transform.localPosition = Vector3.zero;
            sphere.SetActive(true);
            col.enabled = true;
            if (col.radius < grabRadius) col.radius += 12 * Time.fixedDeltaTime;
            sphere.transform.localScale = new Vector3(radius * 2f, radius *2f, radius);
            
        }
        else
        {
            col.enabled = false;
            if(col.radius > 0) col.radius -= 80 * Time.fixedDeltaTime;
            sphere.transform.localScale = new Vector3(radius * 2f, radius *2f, radius);
            var em = particles.emission.rateOverTime;
            em.constant -= 1;
            if (heldObject != null && !grabbing) Launch(launchVelocity);
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
        particles.transform.parent = heldObject.GetTransform();
        particles.transform.localPosition = Vector3.zero;
        particles.Play();
        var em = particles.emission;
        var emRateOverTime = em.rateOverTime;
        emRateOverTime.constant = particleTimeRate;
        var emRateOverDistance = em.rateOverDistance;
        emRateOverDistance.constant = particleDistanceRate;
    }

    private void Launch(float velocity = 0f)
    {
        OnRelease?.Invoke(transform,heldObject);
        heldObject.Release(parentRB.velocity + parentRB.velocity.normalized * velocity);
        joint.connectedBody = null;
        heldObject = null;
        var em = particles.emission.rateOverTime;
        em.constant += 50;

        particles.Play();
        
    }
}
