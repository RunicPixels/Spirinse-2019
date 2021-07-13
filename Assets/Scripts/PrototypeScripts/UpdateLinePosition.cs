using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class UpdateLinePosition : MonoBehaviour
{
    // Throwaway test
    private LineRenderer lineRenderer;
    public Transform originTransform;

    public Transform otherTransform;

    public Transform midTransform;

    
    
    public float curvePosition;
    
    float uvAnimationTileX = 4;
    int uvAnimationTileY = 1;

    public int vertexCount = 12;
    
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }


    public Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        var tangentLineVertex1 = Vector3.Lerp(a, b, t);
        var tangentLineVertex2 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, t);
    }

    public Vector3 CubicCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        var tangentLineVertex1 = QuadraticCurve(a, b, c, t);
        var tangentLineVertex2 = QuadraticCurve(b, c, d, t);
        return Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, t);
    }
    
    void Update()
    {
        Vector3 point1 = transform.position;
        Vector3 point2 = Vector3.Lerp(midTransform.position, transform.position, curvePosition);
        Vector3 point3 = (otherTransform.position + point2) * 0.5f + ((otherTransform.position - transform.position).normalized * (Vector3.Distance(point2, otherTransform.position)) * 0.25f);
        Vector3 point4 = otherTransform.position;

        var pointList = new List<Vector3>();
        
        for (float ratio = 0; ratio <= 1.0f; ratio += 1.0f / vertexCount)
        {
            var bezierPoint = CubicCurve(point1, point2, point3, point4, ratio);
            
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

    void OnDisable()
    {
        lineRenderer.enabled = false;
    }

    void OnEnable()
    {
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
    }

}
