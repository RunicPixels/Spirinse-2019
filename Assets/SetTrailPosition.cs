using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class SetTrailPosition : MonoBehaviour
{
    public Transform trailPosition;
    public TrailRenderer trailRenderer;

    public int index = 40;
    

    private void FixedUpdate()
    {
        if (trailRenderer.positionCount > index)
        {
            trailPosition.position = trailRenderer.GetPosition(index);
        }
        else if(trailRenderer.positionCount > 0)
        {
            trailPosition.position = trailRenderer.GetPosition(trailRenderer.positionCount - 1);
        }
    }
    
}
