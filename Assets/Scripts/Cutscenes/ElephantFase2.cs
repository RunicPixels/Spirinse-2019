using Spirinse.Interfaces;
using Spirinse.System.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantFase2 : MonoBehaviour
{
    public GameObject Particle;

    public GameObject Ibises;    
    public GameObject Wolf;

    public GameObject Thorns;

    public GameObject LightRed;
    public GameObject LightPurple;

    public GameObject MeshCorrupt;
    public GameObject MeshPurified;
    
    public GameObject CutsceneText;

    public BaseEnemy enemy;

    public GameObject Wall;

    public float moveSpeed;

    public int Health = 30;
    public int Damage = 5;

    public bool moveRight;
    public bool noMovement;

    public bool NoTurning;

    public bool sceneActive;

    public Animator elephantController;
    public ParticleSystem hitParticles;

    public float hitCooldown;
    public Canvas TextCanvas;

    private bool hasEnded = false;


    private void Start()
    {
        moveSpeed = 5;

        Ibises.SetActive(false);

        sceneActive = false;
        Particle.SetActive(false);

        noMovement = true;
        NoTurning = false;

        elephantController.SetBool("GetUp", false);
        elephantController.SetBool("Walking", false);
        elephantController.SetBool("Running", false);
        elephantController.SetBool("Falling", false);

        LightRed.SetActive(false);
        LightPurple.SetActive(true);                                                                                                                                                              

        MeshCorrupt.SetActive(true);
        MeshPurified.SetActive(false);

        TextCanvas.enabled = false;
        CutsceneText.SetActive(false);

    }




    void Update()
    {
        if (sceneActive == true)
        {
            StartCoroutine(GetUpToFight());
        }

        if (Health <= 0 && !hasEnded)
        {
            EndCombat();
        }

        if (hasEnded) return;
        if (noMovement)
        {
            transform.Translate(0 * Time.deltaTime * moveSpeed, 0, 0);
        }

        else if (moveRight)
        {
            transform.Translate(-2 * Time.deltaTime * moveSpeed, 0, 0);
            GetComponent<Transform>().eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.Translate(-2 * Time.deltaTime * moveSpeed, 0, 0);
            GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);
        }


    }

    
    private void OnTriggerEnter2D(Collider2D trig)
    {

        if (hasEnded) return;
        if (trig.gameObject.CompareTag("Turn") || trig.gameObject.CompareTag("Enemy"))
        {
            if (moveRight)
            { moveRight = false; }

            else
            { moveRight = true; }
        }


        if (trig.gameObject.CompareTag("Selected") || trig is IDamagable)
        {
            NoTurning = true;
            elephantController.SetBool("Hit", true);

            StartCoroutine(HitReaction());

            Ibises.SetActive(true);

            Particle.SetActive(true);

            TakeDamage(Damage);

            if (Health <= 50)
            {
                elephantController.SetBool("Running", true);
                moveSpeed = 10;
            }

            if (Health <= 0 && !hasEnded)
            {
                EndCombat();
            }
        }

        if (trig.CompareTag(Statics.TagPlayer))
        {
            elephantController.SetBool("GetUp", true);
            sceneActive = true;
        }        
    }

    void EndCombat()
    {
        hasEnded = true;
        Health = 0;
        StartCoroutine(EndOfCombat());
        moveSpeed = 0;
        CutsceneText.SetActive(true);
    }
    private IEnumerator GetUpToFight()
    {
        {
            yield return new WaitForSeconds(5);

            if (hasEnded) yield break;

            noMovement = false;
            elephantController.SetBool("StartWalking", true);
            
            yield return null;            
        }
    }
    public bool TakeDamage(int damage)
    {
        Health -= damage;
        EffectsManager.Instance.timeManager.Freeze(0.05f, 0, 3f, 3f);
        enemy.TakeDamageAction.Invoke();
        hitParticles.Play();

        return true;
    }
    private IEnumerator HitReaction()
    {
        {
            yield return new WaitForSeconds(2);

            if (hasEnded) yield break;

            Particle.SetActive(false);
            Ibises.SetActive(true);
            elephantController.SetBool("Hit", false);
            
            moveSpeed = 0;

            yield return new WaitForSeconds(7);

            if (hasEnded) yield break;

            yield return null;
            elephantController.SetBool("Walking", true);
            NoTurning = false;
            moveSpeed = 5;
        }
    }

    private IEnumerator EndOfCombat()
    {
        {
            enemy.DieAction.Invoke();

            foreach(DamageBlock db in transform.GetComponentsInChildren<DamageBlock>())
            {
                db.activated = false;
            }

            noMovement = true;
            moveRight = true;

            elephantController.SetBool("EndCombat", true);

            Wolf.SetActive(false);
            Ibises.SetActive(false);
            Thorns.SetActive(false);

            LightRed.SetActive(true);
            LightPurple.SetActive(false);

            MeshCorrupt.SetActive(false);
            MeshPurified.SetActive(true);

            TextCanvas.enabled = true;

            Wall.SetActive(false);
            
            yield return null;
        }
    }
}

