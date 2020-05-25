using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElephantFase1 : MonoBehaviour
{
    public Animator ElephantAnim;
    public Animator TreeAnim;

    public GameObject Elephant1;
    public GameObject Elephant2;

    public GameObject ColStand;
    public GameObject ColPrance;

    public GameObject Particle;

    public AudioSource TrumpetNoise;
    public AudioSource BranchNoise;

    public Text SwordDisabledText;

    public int Health = 30;
    public int Damage = 5;

    private void Start()
    {
        ElephantAnim.SetBool("Hit", false);
        ElephantAnim.SetBool("SwordHit", false);
        ElephantAnim.SetBool("FallDownHill", false);
        Elephant1.SetActive(true);
        Elephant2.SetActive(false);

        ColPrance.SetActive(false);
        ColStand.SetActive(false);

        Particle.SetActive(false);

        SwordDisabledText.enabled = false;
    }

    private void Update()
    {
        if (ElephantAnim.GetCurrentAnimatorStateInfo(0).IsName("PranceIdle"))
        {
            ColPrance.SetActive(true);
            ColStand.SetActive(false);
        }

        if (ElephantAnim.GetCurrentAnimatorStateInfo(0).IsName("StandTired"))
        {
            ColStand.SetActive(true);
            ColPrance.SetActive(false);
            StartCoroutine(DownForDamage());
        }
    }

    private void OnTriggerEnter2D(Collider2D elephant)
    {
        if (elephant.CompareTag("Selected") && ElephantAnim.GetCurrentAnimatorStateInfo(0).IsName("PranceIdle"))
        {
            SwordDisabledText.enabled = true;
            StartCoroutine(SwordText());
        }


        if (elephant.CompareTag("Throwable") && ElephantAnim.GetCurrentAnimatorStateInfo(0).IsName("PranceIdle"))
        {
            ElephantAnim.SetBool("Prance", false);
            ElephantAnim.SetBool("Hit", true);
            BranchNoise.Play();
            TrumpetNoise.Play();
            StartCoroutine(DownForDamage());                      
        }

        else if (elephant.CompareTag("Selected") && ElephantAnim.GetCurrentAnimatorStateInfo(0).IsName("StandTired"))
        {
            ElephantAnim.SetBool("SwordHit", true);
            Particle.SetActive(true);
            BranchNoise.Play();
            StartCoroutine(NoMoreLeaves());

            Health -= Damage;
            if (Health <= 0)
            {
                Health = 0;

                ColStand.SetActive(false);
                ColPrance.SetActive(false);

                TreeAnim.SetBool("CutOff", true);
                ElephantAnim.SetBool("FallDownHill", true);
                StartCoroutine(ElephantSwitch());
            }
        }

        else
        {
            ElephantAnim.SetBool("Hit", false);
            ElephantAnim.SetBool("SwordHit", false);
            ElephantAnim.SetBool("Prance", false);
        }
    }


    private IEnumerator ElephantSwitch()
    {
        yield return new WaitForSeconds(2);
        Elephant1.SetActive(false);
        Elephant2.SetActive(true);   
    }


    private IEnumerator DownForDamage()
    {
        yield return new WaitForSeconds(2);
        ElephantAnim.SetBool("Hit", false);
        ElephantAnim.SetBool("SwordHit", false);

        yield return new WaitForSeconds(5);
        ElephantAnim.SetBool("Prance", true);

        yield return null;
    }


    private IEnumerator NoMoreLeaves()
    {
        yield return new WaitForSeconds(2);

        ElephantAnim.SetBool("SwordHit", false);
        Particle.SetActive(false);
    }


    private IEnumerator SwordText()
    {
        yield return new WaitForSeconds(3);
        SwordDisabledText.enabled = false;
        yield return null;
    }

}
