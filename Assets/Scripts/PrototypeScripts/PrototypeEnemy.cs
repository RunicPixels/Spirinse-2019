﻿ using System;
using System.Collections;
using System.Collections.Generic;
using Spirinse.Interfaces;
using UnityEngine;
using Spirinse.Player;
using Spirinse.System;
using Spirinse.System.Effects;
using Spirinse.System.Player;
using Spirinse.Objects;
using FMODUnity;
 using Random = UnityEngine.Random;

public class PrototypeEnemy : MonoBehaviour, IDamagable
{
    public enum EnemyState
    {
        Dashing,
        Idle,
        Moving,
        PreDash,
        PostDash
    }

    public BaseEnemy enemy;
    

    public LineRenderer lr;

    public EnemyState state;

    private float speed;

    public Transform visualContainer;

    public float idleSpeed;
    public float activeSpeed;

    public float health = 5;
    public Animator animator;

    public bool cured;

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

    private readonly int damage = 1;

    public float preDashDuration = 0.4f;
    public float postDashDuration = 0.4f;
    public float dashDuration = 2f;
    public float minDashCooldown = 6f;
    public float maxDashCooldown = 6f;
    public float dashPlayerDistanceMultiplyer = 0.5f;

    private float iFrames;
    private float stunned;
    private float dashTime;
    private float currentDashCooldown;
    private float currentPostDashTime;
    private float currentPlayerDashDistanceDuration;
    private float currentPlayerDistance;

    private float progression = 0f;
    public ParticleSystem hitParticles;

    private static readonly int Active = Animator.StringToHash("Active");
    private const string StrB = "Player";
    

    // Start is called before the first frame update
    void Start()
    {
        //currentDashCooldown = maxDashCooldown;
        enemy = GetComponent<BaseEnemy>();
        state = EnemyState.Moving;
        oldMaterial = meshRenderer.material;
        speed = idleSpeed;
        originalTarget = target;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        CheckCanDoDash();
        // Ugly code, needs refactoring.
        if (!target) target = PlayerManager.Instance.GetPlayer().defender.transform;

        if (iFrames > 0f)
        {
            iFrames -= Time.fixedDeltaTime;
        }

        if (currentDashCooldown > 0f)
        {
            currentDashCooldown -= Time.fixedDeltaTime;
        }

        if (dashTime < 0f && state == EnemyState.Dashing)
        {
            StopDash();
        }

        if (dashTime > 0f)
        {
            dashTime -= Time.fixedDeltaTime;
        }

        if (stunned > 0f)
        {
            stunned -= Time.fixedDeltaTime;
            goto StunJump;
        }

        switch (state)
        {
            case EnemyState.Dashing:
                lr.gameObject.SetActive(false);
                speed = activeSpeed;
                break;
            case EnemyState.Idle:
                speed = idleSpeed;
                // For now
                state = EnemyState.Moving;
                break;
            case EnemyState.PreDash:
                lr.gameObject.SetActive(true);
                speed = 0.5f;
                var position = transform.position;
                lr.SetPosition(0, position);
                lr.SetPosition(1, GetDashEndPos());
                break;
            case EnemyState.Moving:
                speed = idleSpeed;
                direction = (target.position + ((Vector3)target.GetComponent<Rigidbody2D>().velocity * 0.1f) - transform.position).normalized;
                break;
            case EnemyState.PostDash:
                if (currentPostDashTime > 0) currentPostDashTime -= Time.fixedDeltaTime;
                else state = EnemyState.Moving;

                speed = 5 * Mathf.Max(currentPostDashTime - 1.1f, 0f);
                break;

        }

        if (cured) direction = -direction * 0.01f;
        if (animator.transform.localScale.x < 0.05f) { Destroy(); }

        rb.velocity += (Vector2)(direction * speed);

        StunJump:

        //flipped = rb.velocity.x < 0;
        //var xScale = flipped ? 2f : -2f;
        //transform.localScale = new Vector3(2f, xScale, 2f);

        var v = rb.velocity;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        visualContainer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public Vector3 GetDashEndPos()
    {
        var position = transform.position;
        return position + (dashDuration + currentPlayerDashDistanceDuration) * activeSpeed * direction;
    }

    public bool TakeDamage(int damage)
    {
        if (iFrames > 0f || cured || damage < 1) return false;
        health -= damage;
        enemy.TakeDamageAction?.Invoke();
        EffectsManager.Instance.timeManager.Freeze(0.05f, 0, 3f, 3f);
        animator.SetTrigger(Hit);
        hitParticles.Play();

        if (health < 0 && !cured)
        {
            enemy.DieAction?.Invoke();
            //SpawnEnemy.enemyAmount -= 1;
            Cure();
        }

        Stun();
        return true;
    }

    private void Stun()
    {
        rb.velocity = (-speed - 5) * 10f * direction;
        iFrames = 0.3f;
        stunned = 0.4f;
    }

    private void Cure()
    {
        //CleanseManager.Instance.cleanseEvent?.Invoke();
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
        }
    }

    private void CheckCanDoDash()
    {
        if (currentDashCooldown <= 0f &&
            state != EnemyState.Dashing &&
            state != EnemyState.PreDash &&
            state != EnemyState.PostDash)
        {
            StartCoroutine(DoDash());
        }
    }

    private IEnumerator DoDash()
    {
        float thisDashTime = dashDuration;
        enemy.AttackAction.Invoke();
        
        
        while (state != EnemyState.PreDash)
        {
            currentPlayerDashDistanceDuration = CalculatePlayerDashDuration();
            thisDashTime = dashDuration + CalculatePlayerDashDuration();
            state = EnemyState.PreDash;
            yield return new WaitForSeconds(preDashDuration);
        }
        dashTime = thisDashTime;
        animator.SetBool(Active, true);
        state = EnemyState.Dashing;
    }


    public float CalculatePlayerDashDuration()
    {
        if (!target) return 0f;
        float distance = Vector3.Distance(target.position, transform.position);

        float time = distance / activeSpeed;

        return time;

    }

    private void StopDash()
    {
        speed = idleSpeed;
        animator.SetBool(Active, false);
        currentDashCooldown = Random.Range(minDashCooldown, maxDashCooldown);
        currentPostDashTime = postDashDuration;
        state = EnemyState.PostDash;
    }
}
