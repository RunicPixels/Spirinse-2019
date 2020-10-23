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

    public GameObject Wall;

    public float moveSpeed;

    public int Health = 30;
    public int Damage = 5;

    public bool moveRight;
    public bool noMovement;

    public bool NoTurning;

    public bool sceneActive;

    public Animator elephantController;
    
    public Canvas TextCanvas;


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

        if (Health <= 0)
            {
                Health = 0;
                StartCoroutine(EndOfCombat());
                moveSpeed = 0;
                CutsceneText.SetActive(true);
            }
    }

    
    private void OnTriggerEnter2D(Collider2D trig)
    {

        if (trig.gameObject.CompareTag("Turn") || trig.gameObject.CompareTag("Enemy"))
        {
            if (moveRight)
            { moveRight = false; }

            else
            { moveRight = true; }
        }


        if (trig.gameObject.CompareTag("Selected"))
        {
            NoTurning = true;
            elephantController.SetBool("Hit", true);

            StartCoroutine(HitReaction());

            Ibises.SetActive(true);

            Particle.SetActive(true);

            Health -= Damage;

            if (Health <= 50)
            {
                elephantController.SetBool("Running", true);
                moveSpeed = 10;
            }

            if (Health <= 0)
            {
                Health = 0;
                StartCoroutine(EndOfCombat());
            }
        }

        if (trig.CompareTag(Statics.TagPlayer))
        {
            elephantController.SetBool("GetUp", true);
            sceneActive = true;
        }        
    }

    private IEnumerator GetUpToFight()
    {
        {
            yield return new WaitForSeconds(5);

            noMovement = false;
            elephantController.SetBool("StartWalking", true);
            
            yield return null;            
        }
    }

    private IEnumerator HitReaction()
    {
        {
            yield return new WaitForSeconds(2);

            Particle.SetActive(false);
            Ibises.SetActive(true);
            elephantController.SetBool("Hit", false);
            
            moveSpeed = 0;

            yield return new WaitForSeconds(7);

            yield return null;
            elephantController.SetBool("Walking", true);
            NoTurning = false;
            moveSpeed = 5;
        }
    }

    private IEnumerator EndOfCombat()
    {
        {
            yield return new WaitForSeconds(1);

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
