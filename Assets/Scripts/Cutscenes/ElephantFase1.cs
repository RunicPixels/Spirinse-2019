using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantFase1 : MonoBehaviour
{

    public Animator ElephantAnim;
    public Animator TreeAnim;

    public GameObject Elephant1;
    public GameObject Elephant2;

    public int Health = 30;
    public int Damage = 5;

    private void Start()
    {
        ElephantAnim.SetBool("Hit", false);
        ElephantAnim.SetBool("SwordHit", false);
        ElephantAnim.SetBool("FallDownHill", false);
        Elephant1.SetActive(true);
        Elephant2.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D elephant)
    {
        if (elephant.CompareTag("Throwable") && ElephantAnim.GetCurrentAnimatorStateInfo(0).IsName("PranceIdle"))
        {
            ElephantAnim.SetBool("Prance", false);
            ElephantAnim.SetBool("Hit", true);
            StartCoroutine(DownForDamage()); 
        }

        else if (elephant.CompareTag("Selected") && ElephantAnim.GetCurrentAnimatorStateInfo(0).IsName("StandTired"))
        {
            ElephantAnim.SetBool("SwordHit", true);
            Health -= Damage;
            if (Health <= 0)
            {
                Health = 0;
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
        yield return new WaitForSeconds(10);
        ElephantAnim.SetBool("Prance", true);
    }
}
