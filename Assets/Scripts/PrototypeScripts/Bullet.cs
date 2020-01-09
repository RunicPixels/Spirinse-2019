using System;
using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour, IAttack
{
    private Vector2 direction;
    private Rigidbody2D rb;
    public float velocity = 5f;
    public float timeLeft = 2f;
    public int damage = 6;
    public DisappearAfterTime vfx;
    
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void Init(Vector2 direction)
    {
        this.direction = direction;
    }
    
    // Update is called once per frame
    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f)
        {
            Destroy();
        }
        
        Vector2 v = rb.velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        IDamagable hit = other.gameObject.GetComponent<IDamagable>();

        if (hit != null)
        {
            hit.TakeDamage(damage);
        }
        Destroy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable hit = collision.gameObject.GetComponent<IDamagable>();

        if (hit != null)
        {
            Destroy();
        }
        
    }
    public void Destroy()
    {
        vfx.transform.parent = null;
        vfx.enabled = true;
        Destroy(gameObject);
    }

    public int DoAttack()
    {
        return damage;
    }
}
