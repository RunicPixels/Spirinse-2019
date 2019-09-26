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

    float uvAnimationTileX = 4;
    int uvAnimationTileY = 1;

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
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,otherTransform.position);
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
