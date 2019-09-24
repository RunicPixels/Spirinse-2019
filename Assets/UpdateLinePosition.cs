using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;

public class UpdateLinePosition : MonoBehaviour
{
    // Throwaway test
    private LineRenderer lineRenderer;
    private Transform thisTransform;

    public Transform otherTransform;

    // Start is called before the first frame update
    void Start()
    {
        thisTransform = transform;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,otherTransform.position);
    }
}
