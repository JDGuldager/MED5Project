    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator animator;

        public void HitTrigger()
    {
        animator.SetTrigger("ShowExer");
    }  
    
    public void Exer1()
    {
        animator.SetBool("Exer1",true);
        animator.SetBool("Exer2", false);
    }

    public void Exer2()
    {
        animator.SetBool("Exer2", true);
        animator.SetBool("Exer1", false);
    }

    public void RightArm()
    {
        animator.SetBool("RightArm", true);
        animator.SetBool("LeftArm", false);
    }

    public void LeftArm()
    {
        animator.SetBool("LeftArm", true);
        animator.SetBool("Right", false);
    }
}
