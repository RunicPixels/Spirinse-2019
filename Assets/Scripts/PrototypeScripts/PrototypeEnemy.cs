using System;
using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;
using Spirinse.Player;
public class PrototypeEnemy : MonoBehaviour, IDamagable
{
    private const string StrB = "Player";
    public float speed;
    public float health = 5;
    public Animator animator;

    public bool cured = false;

    private Vector3 direction;

    private Rigidbody2D rb;

    public Transform target;
    private Transform originalTarget;

    private static readonly int Cure1 = Animator.StringToHash("Cure");
    private static readonly int Hit = Animator.StringToHash("Hit");

    private bool flipped;

    private int damage = 1;

    public float iFrames = 0f;
    public float stunned = 0;

    public ParticleSystem hitParticles;

    // Start is called before the first frame update
    void Start()
    {
        originalTarget = target;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!target) target = Meditator.Instance.transform;
        if (iFrames > 0f)
        {
            iFrames -= Time.fixedDeltaTime;
        }

        if (stunned > 0f)
        {
            stunned -= Time.fixedDeltaTime;
            goto StunJump;
        }

        direction = (target.position - transform.position).normalized;

        rb.velocity = direction * speed;

        if (cured)
        {
            rb.velocity += Vector2.up * 1.5f;

        }

        StunJump:

        flipped = rb.velocity.x < 0;


        var xScale = flipped ? 2f : -2f;

        transform.localScale = new Vector3(2f, xScale, 2f);


        var v = -rb.velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void TakeDamage(int damage)
    {
        if (iFrames > 0f) return;
        health -= damage;
        animator.SetTrigger(Hit);

        Stun();

    }

    private void Stun()
    {
        rb.velocity = direction * -speed * 2f;
        iFrames = 0.3f;
        hitParticles.Play();
        stunned = 0.4f;
        if (health < 0)
        {
            Cure();
        }
    }

    private void Cure()
    {
        cured = true;
        animator.SetTrigger(Cure1);
        transform.gameObject.layer = LayerMask.NameToLayer("NoCollision");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Defender") && Spirinse.System.Health.HealthManager.Instance.ShieldManager.GetShield() > 0)
        {
            target = other.transform;
        }
        else if(other.CompareTag("Defender"))
        {
            target = originalTarget;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Defender"))
        {
            target = originalTarget;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D myCollider = collision.GetContact(0).collider;

        MonoBehaviour[] list = myCollider.gameObject.GetComponents<MonoBehaviour>();


        foreach(MonoBehaviour mb in list)
        {
            if (mb is IDamagable)
            {
                Debug.Log("Hitting Damageable");
                IDamagable damageable = (IDamagable)mb;
                
                damageable.TakeDamage(damage);
                Stun();
            }
            if (mb is IAttack && iFrames <= 0)
            {
                Debug.Log("Hitting Attack");
                IAttack attack = (IAttack)mb;

                TakeDamage(attack.DoAttack());
            }
        }
    }
}
