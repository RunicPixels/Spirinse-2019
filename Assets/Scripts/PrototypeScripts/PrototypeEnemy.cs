using System;
using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;
using Spirinse.Player;
using Spirinse.System;
using Spirinse.System.Effects;

public class PrototypeEnemy : MonoBehaviour, IDamagable
{
    private const string StrB = "Player";
    private float speed;

    public float idleSpeed;
    public float activeSpeed;

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
        speed = idleSpeed;
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
        if (cured) direction = -direction;

        rb.velocity = direction * speed;

        if (cured)
        {
            rb.velocity += Vector2.up * 1.5f;
        }

        StunJump:

        flipped = rb.velocity.x < 0;


        var xScale = flipped ? 2f : -2f;

        transform.localScale = new Vector3(2f, xScale, 2f);


        var v = rb.velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void TakeDamage(int damage)
    {
        if (iFrames > 0f || cured) return;
        health -= damage;
        TimeManager.Instance.Freeze(0.05f, 0, 3f, 3f);
        //animator.SetTrigger(Hit);

        if (health < 0 && !cured)
        {
            SpawnEnemy.enemyAmount -= 1;
            Cure();
        }

        Stun();

    }

    private void Stun()
    {
        rb.velocity = direction * -speed * 2f;
        iFrames = 0.3f;
        stunned = 0.4f;

    }

    private void Cure()
    {
        CleanseManager.Instance.cleanseEvent?.Invoke();
        
        cured = true;
        //animator.SetTrigger(Cure1);
        transform.gameObject.layer = LayerMask.NameToLayer("NoCollision");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Defender") && Spirinse.System.Health.HealthManager.Instance.ShieldManager.GetShield() > 0)
        {
            target = other.transform;
            speed = activeSpeed;
            animator.SetBool("Active", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Defender"))
        {
            target = originalTarget;
            speed = idleSpeed;
            animator.SetBool("Active", false);
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
                IDamagable damageable = (IDamagable)mb;
                hitParticles.Play();
                damageable.TakeDamage(damage);
                Stun();
            }
            if (mb is IAttack && iFrames <= 0)
            {
                IAttack attack = (IAttack)mb;

                TakeDamage(attack.DoAttack());
            }
        }
    }
}
