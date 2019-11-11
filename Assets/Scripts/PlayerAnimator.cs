using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    // Start is called before the first frame update

    public void ChangeAnimation(bool state)
    {
        if(anim.GetBool("Flying") == !state)anim.SetBool("Flying", state);
    }
}
