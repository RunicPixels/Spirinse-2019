using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using MEC;

public class ResetCharacterPosition : MonoBehaviour
{
    public Character character;
    public GameObject modelContainer;

    public Rigidbody2D rb;

    private Character.CharacterTypes type;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(character == null)character = GetComponent<Character>();
        type = character.CharacterType;

        Timing.RunCoroutine(_UpdateCharacterState());
    }

    IEnumerator<float> _UpdateCharacterState()
    {
        while (gameObject.activeSelf)
        {
            while (character.CharacterType == type)
            {
                yield return 0f;
            }
            
            type = character.CharacterType;

            switch (type)
            {
                case Character.CharacterTypes.AI:
                    rb.Sleep();
                    character.transform.localPosition = Vector3.zero;
                    modelContainer.SetActive(false);
                    break;

                case Character.CharacterTypes.Player:
                    rb.WakeUp();
                    character.transform.localPosition = Vector3.zero;
                    modelContainer.SetActive(true);
                    break;

                default:
                    break;
            }

            yield return 0f;
        }
    }


}
