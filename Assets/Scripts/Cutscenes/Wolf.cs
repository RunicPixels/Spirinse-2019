using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Interfaces;
using Spirinse.System.Effects;

public class Wolf : MonoBehaviour
{
    public float moveSpeed;
    public BaseEnemy enemy;
    public GameObject wolf;
    public GameObject Particle;

    public bool moveRight;
    public bool noMovement;

    public Animator wolfAnimator;

    public int Health = 30;
    private readonly int damage = 1;
    public float hitCooldown = 0.5f;
    public float beingHitCooldown = 0.3f;
    private float currentHitCooldown;
    private float currentBeingHitCooldown;
    public ParticleSystem hitParticles;

    void Start()
    {
        enemy = GetComponent<BaseEnemy>();
        wolfAnimator.SetBool("Attack", false);
        wolfAnimator.SetBool("hit", false);
        noMovement = false;
        Particle.SetActive(false);
    }


    void Update()
    {
        //Walking Controll//
        if (noMovement)
        {
            transform.Translate(0 * Time.deltaTime * moveSpeed, 0, 0);
            wolfAnimator.SetBool("Moving", false);
        }

        else if (moveRight)
        {
            transform.Translate(-2 * Time.deltaTime * moveSpeed, 0, 0);
            GetComponent<Transform>().eulerAngles = new Vector3(0, 180, 0);
            wolfAnimator.SetBool("Moving", true);
        }
        else
        {
            transform.Translate(-2 * Time.deltaTime * moveSpeed, 0, 0);
            GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);
            wolfAnimator.SetBool("Moving", true);
        }
        if (currentHitCooldown > 0f) currentHitCooldown -= Time.deltaTime;
        if (currentBeingHitCooldown > 0f) currentBeingHitCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        //Turning around Controll//
        if (trig.gameObject.CompareTag(Statics.TagTurn) || trig.gameObject.CompareTag(Statics.TagEnemy))
        {
            TurnAround();
        }

        MonoBehaviour[] list = trig.gameObject.GetComponents<MonoBehaviour>();

        foreach (var mb in list)
        {
            if (mb is IDamagable && currentHitCooldown <= 0.01f) //Check if the wolf is ATTACKING the player//
            {
                currentHitCooldown = hitCooldown;
                IDamagable damageable = (IDamagable)mb;
                damageable.TakeDamage(damage);
                noMovement = true;
            }
            if (mb is IAttack && currentBeingHitCooldown <= 0.01f) //Check if the wolf is BEING attacked BY the player//
            {
                currentBeingHitCooldown = beingHitCooldown;
                IAttack attack = (IAttack)mb;

                TakeDamage(attack.DoAttack());
            }
        }
        
        if (!trig.gameObject.CompareTag("Selected"))
        {
            wolfAnimator.SetBool("Hit", false);
            Particle.SetActive(false);
        }
        
       
    }

    private void TurnAround ()
    {
        if (moveRight)
        { moveRight = false; }

        else
        { moveRight = true; }
    }


    private void TakeDamage(int takenDamage)
    {
        Health -= takenDamage;
        Particle.SetActive(true);
        TurnAround();
        StartCoroutine(NoMoreLeaves());
        enemy.TakeDamageAction?.Invoke();
        EffectsManager.Instance.timeManager.Freeze(0.05f, 0, 3f, 3f);
        hitParticles.Play();


        if (Health <= 0)
        {
            Health = 0;
            enemy.DieAction?.Invoke();
            wolf.SetActive(false);
        }
    }
    
    private void OnTriggerExit2D(Collider2D trig)
    {
        MonoBehaviour[] list = trig.gameObject.GetComponents<MonoBehaviour>();

        foreach (var mb in list) // START MOVING AGAIN
        {
            if (mb is IDamagable) 
            {
                noMovement = false;
            }
        }
    }

    private IEnumerator NoMoreLeaves()
    {
        yield return new WaitForSeconds(1);

        wolfAnimator.SetBool("Hit", false);
        Particle.SetActive(false);
    }
}

