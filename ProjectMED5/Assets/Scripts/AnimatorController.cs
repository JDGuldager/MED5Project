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
    }

    public void Exer2()
    {
        animator.SetBool("Exer1", false);
    }


}
