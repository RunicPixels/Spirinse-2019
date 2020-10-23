using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spirinse.Interfaces;

public class Wolf : MonoBehaviour
{
    public float moveSpeed;

    public GameObject wolf;
    public GameObject Particle;

    public bool moveRight;
    public bool noMovement;

    public Animator wolfAnimator;

    public int Health = 30;
    private readonly int damage = 1;

    void Start()
    {
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
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        //Turning around Controll//
        if (trig.gameObject.CompareTag(Statics.TagTurn) || trig.gameObject.CompareTag(Statics.TagEnemy))
        {
            if (moveRight)
            { moveRight = false; }

            else
            { moveRight = true; }
        }

        MonoBehaviour[] list = trig.gameObject.GetComponents<MonoBehaviour>();

        foreach (var mb in list)
        {
            if (mb is IDamagable) //Check if the wolf is ATTACKING the player//
            {
                IDamagable damageable = (IDamagable)mb;
                damageable.TakeDamage(damage);
                noMovement = true;
            }
            if (mb is IAttack) //Check if the wolf is BEING attacked BY the player//
            {
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

    private void TakeDamage(int takenDamage)
    {
        Health -= takenDamage;
        Particle.SetActive(true);
        StartCoroutine(NoMoreLeaves());

        if (Health <= 0)
        {
            Health = 0;
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

