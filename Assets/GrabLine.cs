using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;

[RequireComponent(typeof(Grab))][RequireComponent(typeof(UpdateLinePosition))]
public class GrabLine : MonoBehaviour
{
    private Grab grab;

    private UpdateLinePosition lp;
    // Start is called before the first frame update
    void Start()
    {
        grab = GetComponent<Grab>();
        lp = GetComponent<UpdateLinePosition>();
    }

    void OnEnable()
    {
        if(!grab) grab = GetComponent<Grab>();
        grab.OnGrab += DoLine;
        grab.OnRelease += StopLine;
    }

    void OnDisable()
    {
        grab.OnGrab -= DoLine;
        grab.OnRelease -= StopLine;
    }
    
    void DoLine(Transform origin, IGrabbable destination)
    {
         lp.enabled = true;
         lp.originTransform = origin;
         lp.otherTransform = destination.GetTransform();
     }
 
     void StopLine(Transform origin, IGrabbable destination)
     {
         lp.originTransform = null;
         lp.otherTransform = null;
         lp.enabled = false;
     }
 }
