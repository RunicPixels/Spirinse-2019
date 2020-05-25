using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;

public class GrabLineMidPos : MonoBehaviour
{
    public Grab grab;

    private Rigidbody2D rb;

    public float magnitude;
    private Transform origin;
    private bool doMove = false;
    // Start is called before the first frame update
    void Start()
    {
        if(grab == null) Debug.LogError("No Grab Assigned to Grab Line Mid Pos");
    }

    void OnEnable()
    {
        if(!grab) grab = GetComponent<Grab>();
        grab.OnGrab += DoLine;
        grab.OnRelease += StopLine;
    }

    void Update()
    {
        if(doMove)transform.position = ((rb.transform.position + origin.position) * 0.5f) + (Vector3)(rb.velocity * magnitude);
        
    }

    void OnDisable()
    {
        grab.OnGrab -= DoLine;
        grab.OnRelease -= StopLine;
    }
    
    void DoLine(Transform origin, IGrabbable destination)
    {
        rb = destination.GetRigidbody2D();
        this.origin = origin;
        doMove = true;
    }

    void StopLine(Transform origin, IGrabbable destination)
    {
        rb = null;
        this.origin = null;
        doMove = false;
    }
}
