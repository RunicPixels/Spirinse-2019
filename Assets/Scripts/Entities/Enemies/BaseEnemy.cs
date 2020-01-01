using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.System;
using Spirinse.System.Effects;
using Spirinse.Interfaces;

public class BaseEnemy : MonoBehaviour, IDamagable
{
    public enum EnemyState
    {
        Dashing,
        Idle,
        Moving,
        PreDash
    }


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
    public float dashDuration = 2f;
    public float minDashCooldown = 6f;
    public float maxDashCooldown = 6f;
    public float dashPlayerDistanceMultiplyer = 0.5f;

    private float iFrames;
    private float progression = 0f;
    public ParticleSystem hitParticles;

    private static readonly int Active = Animator.StringToHash("Active");
    private const string StrB = "Player";

    // Start is called before the first frame update
    void Start()
    {
        //currentDashCooldown = maxDashCooldown;
    }
      

    public void TakeDamage(int damage)
    {
        if (iFrames > 0f || cured || damage < 1) return;
        health -= damage;
        //EffectsManager.Instance.timeManager.Freeze(0.05f, 0, 3f, 3f);
        animator.SetTrigger(Hit);
        hitParticles.Play();

        if (health < 0 && !cured)
        {
            //SpawnEnemy.enemyAmount -= 1;
            Cure();
        }

        //Stun();
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
                //Stun();
            }
            if (mb is IAttack && iFrames <= 0)
            {
                IAttack attack = (IAttack)mb;

                TakeDamage(attack.DoAttack());
            }
        }
    }
}