using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantFase2 : MonoBehaviour
{
    public float speed;

    public bool moveRight;

    public bool noMovement;

    public Animator elephantController;


    private void Start()
    {
        noMovement = true;
        elephantController.SetBool("GetUp", false);
    }


    void Update()
    {
        if (noMovement)
        {
            transform.Translate(0 * Time.deltaTime * speed, 0, 0);
            elephantController.SetBool("Walking", false);
        }

        else if (moveRight)
        {
            transform.Translate(-2 * Time.deltaTime * speed, 0, 0);
            GetComponent<Transform>().eulerAngles = new Vector3(0, 180, 0);
            elephantController.SetBool("Walking", true);
        }
        else
        {
            transform.Translate(-2 * Time.deltaTime * speed, 0, 0);
            GetComponent<Transform>().eulerAngles = new Vector3(0, 0, 0);
            elephantController.SetBool("Walking", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.CompareTag("Defender"))
        {
            elephantController.SetBool("GetUp", true);
            StartCoroutine(GetUpToFight());
        }

        if (trig.gameObject.CompareTag("Turn")){

            if (moveRight)
            { moveRight = false; }

            else
            { moveRight = true; }
        }
    }

    private IEnumerator GetUpToFight()
    {
        {
            yield return new WaitForSeconds(4);
            noMovement = false;
        }
    }
}
