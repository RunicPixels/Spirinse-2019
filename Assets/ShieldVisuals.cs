using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldVisuals : MonoBehaviour
{
    public MeshRenderer mesh;

    public void SetShieldVisuals(int state)
    {
        if(state <= 0) {        mesh.enabled = false;}
        else {                  mesh.enabled = true; }
    }
}
