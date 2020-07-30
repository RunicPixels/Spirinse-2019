using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ibis : MonoBehaviour
{
    public Animator Anim;

    public GameObject ibises;

    public int moveSpeed;

    public bool moveRight;


    public void Update()
    {
        Anim.SetBool("Moving", true);

        if (moveRight)
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
        if (trig.gameObject.CompareTag("Turn"))
        {
            if (moveRight)
            {
                moveRight = false;
            }

            else
            {
                moveRight = true;
            }

            ibises.SetActive(false);
        }
    }
}