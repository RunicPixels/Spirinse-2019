using System;
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

    public float curvePosition, curveIntensity;
    
    float uvAnimationTileX = 4;
    int uvAnimationTileY = 1;
    [SerializeField]
    private float maxRBVelocity = 50;

    public int vertexCount = 12;
    
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = transform;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb = otherTransform.GetComponent<Rigidbody2D>();

        Vector3 velocityMax = maxRBVelocity < rb.velocity.magnitude ? rb.velocity.normalized * maxRBVelocity : rb.velocity;
        
        Vector3 point1 = transform.position;
        Vector3 point2 = Vector3.Lerp(otherTransform.position, transform.position, curvePosition) - velocityMax * curveIntensity;
        Vector3 point3 = otherTransform.position;

        var pointList = new List<Vector3>();
        
        for (float ratio = 0; ratio <= 1.0f; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1, point2, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point2, point3, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            
            pointList.Add(bezierPoint);
            
        }

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
        
        
        uvAnimationTileX = Vector3.Distance(transform.position, otherTransform.transform.position);

        // Calculate index
        float index = (Time.time) * speed * uvAnimationTileX;
        index = index % uvAnimationTileX;
        // Size of every tile
        var size = new Vector2(1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY);

        //var vIndex = index / uvAnimationTileX;

        // build offset
        // v coordinate is the bottom of the image in opengl so we need to invert.
        var offset = new Vector2(index / uvAnimationTileX, 0);

        lineRenderer.material.SetTextureOffset("_MainTex", offset);
        //lineRenderer.material.SetTextureScale("_MainTex", size);
    }
}
