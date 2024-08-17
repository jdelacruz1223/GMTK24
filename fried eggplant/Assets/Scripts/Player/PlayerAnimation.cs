using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;


public class PlayerAnimation : MonoBehaviour
{
    //luke
    //     animator.SetBool("isRunning", false);
    //     animator.SetBool("isIdling", false);
    //     animator.SetBool("isJumping", false);
    //     animator.SetBool("isFalling", false);
    //     animator.SetBool("hasBook", false);
    public enum MoveState
    {
        idle,
        running,
        landed,
        falling,
        jumping,
        hasBook,
        hasTurned
    }
    public Animator anim;
    
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimationUpdate(MoveState moveState, bool book)
    {
        if (book) anim.SetBool("hasBook", true);
        switch (moveState)
        {
            case MoveState.idle:
                anim.SetBool("isIdling",true);
                anim.SetBool("isRunning",false);
                anim.SetBool("isFalling",false);
                anim.SetBool("hasLanded",false);
            break;
            case MoveState.running:
                anim.SetBool("isRunning",true);
                anim.SetBool("isIdling",false);
            break;
            case MoveState.landed:
                anim.SetBool("hasLanded", true);
                anim.SetBool("isFalling", false);
                
            break;
            case MoveState.falling:
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping", false);
            break;
            case MoveState.jumping:
                anim.SetBool("isJumping", true);
                anim.SetBool("isIdling", false);
                anim.SetBool("isRunning", false);
            break;
            case MoveState.hasTurned:
                anim.SetBool("isRunning",true);
                anim.SetTrigger("hasTurned");
            break;
            default:
            break;
        }
    }
}
