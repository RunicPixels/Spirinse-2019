using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeEnemy : MonoBehaviour
{
    public float speed;

    private Vector3 direction;

    private Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = (Meditator.Instance.transform.position - transform.position).normalized;

    }

    void FixedUpdate()
    {
        rigidbody.velocity = direction * speed;
    }
}
