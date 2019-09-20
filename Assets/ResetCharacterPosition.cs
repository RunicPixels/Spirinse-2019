using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using MEC;

public class ResetCharacterPosition : MonoBehaviour
{
    Character character;

    private Character.CharacterTypes type;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
        type = character.CharacterType;
        rb = GetComponent<Rigidbody2D>();

        Timing.RunCoroutine(_UpdateCharacterState());
    }

    IEnumerator<float> _UpdateCharacterState()
    {
        while (gameObject.activeSelf)
        {
/*            while (character.CharacterType == type)
            {
                yield return 0f;
            }*/

            type = character.CharacterType;

            switch (type)
            {
                case Character.CharacterTypes.AI:
                    rb.Sleep();
                    transform.localPosition = Vector3.zero;
                    break;

                case Character.CharacterTypes.Player:
                    rb.WakeUp();
                    break;

                default:
                    break;
            }

            yield return 0f;
        }
    }


}
