using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWhenClose : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public float magnitude = 0.2f;
    private Material mat;
    private Color matColor;
    // Start is called before the first frame update
    void Start()
    {
        mat = spriteRenderer.material;
        matColor = mat.color;
    }

    // Update is called once per frame
    void Update()
    {
        Color col = new Color(matColor.r, matColor.g, matColor.b, rb.velocity.magnitude * magnitude);
        mat.color = col;
    }


}
