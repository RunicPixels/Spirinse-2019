using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    public float moveSpeed;

    public GameObject wolf;
    public GameObject Particle;

    public bool moveRight;
    public bool noMovement;

    public Animator wolfAnimator;

    public int Health = 30;
    public int Damage = 5;


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
        if (trig.gameObject.CompareTag("Turn") || trig.gameObject.CompareTag("Enemy"))
        {
            if (moveRight)
            { moveRight = false; }

            else
            { moveRight = true; }
        }


        //Check if the wolf is BEING attacked BY the player//
        if (trig.gameObject.CompareTag("Selected"))
        {
            wolfAnimator.SetBool("Hit", true);
            Health -= Damage;
            Particle.SetActive(true);
            StartCoroutine(NoMoreLeaves());

            if (Health <= 0)
            {
                Health = 0;
                wolf.SetActive(false);
            }
        }         

        if (!trig.gameObject.CompareTag("Selected"))
        {
            wolfAnimator.SetBool("Hit", false);
            Particle.SetActive(false);
        }


        //Check if the wolf is ATTACKING the player//
        if (trig.CompareTag("Defender"))
        {
            noMovement = true;
        }
       
    }

    private void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.CompareTag("Defender"))
        {
            noMovement = false;
        }
    }

    private IEnumerator NoMoreLeaves()
    {
        yield return new WaitForSeconds(1);

        wolfAnimator.SetBool("Hit", false);
        Particle.SetActive(false);
    }
}

