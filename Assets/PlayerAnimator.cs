using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    // Start is called before the first frame update

    public void ChangeAnimation(float speed)
    {
        anim.SetBool("Flying", speed > 0.1f);
    }
}
