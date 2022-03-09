using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour{

    public Text textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public Animator myAnimationController;

    public Animator myAnimationControllerTree;

    public Image backgroundImage;

    public GameObject continueButton;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        textDisplay.enabled = true;
        backgroundImage.enabled = true;
        StartCoroutine(Type());
    }

    private void Update()
    {
      if(textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
    }

    IEnumerator Type() {

        //This delays the next scentence by two seconds
        //yield return new WaitForSeconds(2f);

        foreach(char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence(){
        source.Play();
        continueButton.SetActive(false);

        if (index < sentences.Length - 1){
                index++;
                textDisplay.text = "";
                StartCoroutine(Type());
        } else {
            textDisplay.text = "";
            continueButton.SetActive(false);
            backgroundImage.enabled = false;

            myAnimationController.SetBool("Prance", true);
            myAnimationControllerTree.SetBool("PullBack", true);
            enabled = false;

        }

    }

}
