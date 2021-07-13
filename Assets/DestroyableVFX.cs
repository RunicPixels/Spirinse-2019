using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyableVFX : MonoBehaviour
{
    public VisualEffect vfx;

    public Color effectColor;

    public float height;
    public float width;

    private float usedHeight;
    private float usedWidth;
    // Start is called before the first frame update
    void OnEnable()
    {
        usedHeight = height * transform.lossyScale.y;
        usedWidth = width * transform.lossyScale.x;
    }

    private void OnDrawGizmosSelected()
    {
        Color col = new Color(effectColor.r, effectColor.g, effectColor.b, 0.1f);
        Gizmos.color = col;
        Vector3 box = new Vector3(width,height);
        Gizmos.DrawCube(transform.position, box);
    }
}
