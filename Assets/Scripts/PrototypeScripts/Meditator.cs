using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meditator : MonoBehaviour
{
    public static Meditator Instance;

    public LayerMask hitLayers;

    public float iFrames = 1f;

    private float iFramesCD;
    // Start is called before the first frame update
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }

    private void Update()
    {
        if (iFramesCD > 0f) iFramesCD -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (iFramesCD > 0f) return;
        var enemy = other.GetComponent<PrototypeEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(0);
            Grow.currentGrowth -= 1f;
            iFramesCD = 1f;
        }
    }
}
