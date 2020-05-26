using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationTrue : MonoBehaviour
{
    public PlayerAnimator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim.ChangeAnimation(true);
    }
}
