using System;
using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;
using Spirinse.Player;
using Spirinse.System;
using Spirinse.System.Effects;
using Spirinse.Objects;

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

    public SkinnedMeshRenderer meshRenderer;
    public Material cleansedMaterial;
    private Material oldMaterial;

    private static readonly int Cured = Animator.StringToHash("Cured");
    private static readonly int Hit = Animator.StringToHash("Hit");

    private bool flipped;

    private int damage = 1;

    public float iFrames = 0f;
    public float stunned = 0;

    private float progression = 0f;
    public ParticleSystem hitParticles;

    // Start is called before the first frame update
    void Start()
    {
        oldMaterial = meshRenderer.material;
        speed = idleSpeed;
        originalTarget = target;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Ugly code, needs refactoring.
        if (!target) target = PlayerManager.Instance.player.defender.transform;

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
        if (cured) direction = -direction * 0.01f;
        if(animator.transform.localScale.x < 0.05f) { Destroy(); }

        rb.velocity = direction * speed;

    StunJump:

        flipped = rb.velocity.x < 0;


        var xScale = flipped ? 2f : -2f;

        //transform.localScale = new Vector3(2f, xScale, 2f);


        var v = rb.velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void TakeDamage(int damage)
    {
        if (iFrames > 0f || cured || damage < 1) return;
        health -= damage;
        TimeManager.Instance.Freeze(0.05f, 0, 3f, 3f);
        animator.SetTrigger(Hit);
        hitParticles.Play();

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
        animator.SetTrigger(Cured);
        meshRenderer.material = cleansedMaterial;
        transform.gameObject.layer = LayerMask.NameToLayer("Invulnerable");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        MonoBehaviour[] list = collision.gameObject.GetComponents<MonoBehaviour>();

        foreach (var mb in list)
        {
            if (mb is IDamagable)
            {
                IDamagable damageable = (IDamagable)mb;
                damageable.TakeDamage(damage);
                Stun();
            }
            if (mb is IAttack && iFrames <= 0)
            {
                IAttack attack = (IAttack)mb;

                TakeDamage(attack.DoAttack());
            }
            if (mb is BaseGrabObject && collision.GetComponent<Rigidbody2D>())
            {
                BaseGrabObject grabObject = (BaseGrabObject)mb;
                TakeDamage(grabObject.GetDamage());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Defender"))
        {
            speed = activeSpeed;
            animator.SetBool("Active", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Defender"))
        {
            speed = idleSpeed;
            animator.SetBool("Active", false);
        }
    }
}
