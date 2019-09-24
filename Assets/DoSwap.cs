using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using MEC;

public class DoSwap : MonoBehaviour
{
    private Character.CharacterTypes type;
    private int layerMask;
    private LineRenderer renderer;

    public CharacterSwapManager charSwapManager;
    Character character;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    private void Start()
    {
        character = GetComponent<Character>();
        type = character.CharacterType;
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<LineRenderer>();
        renderer.enabled = false;
        charSwapManager = FindObjectOfType<CharacterSwapManager>();
        layerMask = LayerMask.GetMask("Interactable");
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
                    renderer.enabled = true;
                    break;

                case Character.CharacterTypes.Player:
                    renderer.enabled = false;
                    break;

                default:
                    break;
            }

            yield return 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 3f, layerMask);
            if (hit.collider != null)
            {
                if (hit.transform.CompareTag("Meditation"))
                {
                    charSwapManager.SwapCharacter();
                    renderer.enabled = true;
                    transform.position = hit.transform.position + Vector3.up;
                }
            }
        }    
    }


}
