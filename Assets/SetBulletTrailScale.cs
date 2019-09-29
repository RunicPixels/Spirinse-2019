using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBulletTrailScale : MonoBehaviour
{
    private TrailRenderer _trailRenderer;
        
    // Start is called before the first frame update
    void Start()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        var lossyScale = transform.lossyScale;
        _trailRenderer.widthMultiplier = lossyScale.y;
        _trailRenderer.time *= lossyScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
