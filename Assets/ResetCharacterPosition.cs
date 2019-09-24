using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using MEC;

public class ResetCharacterPosition : MonoBehaviour
{
    public Character character;

    private Character.CharacterTypes type;

    // Start is called before the first frame update
    void Start()
    {
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
                    character.gameObject.SetActive(false);
                    character.transform.localPosition = Vector3.zero;
                    break;

                case Character.CharacterTypes.Player:
                    character.gameObject.SetActive(true);
                    character.transform.localPosition = Vector3.zero;
                    break;

                default:
                    break;
            }

            yield return 0f;
        }
    }


}
