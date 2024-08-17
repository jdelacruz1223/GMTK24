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
                anim.SetBool("isIdling", true);
            break;
            case MoveState.running:
                anim.SetBool("isRunning", true);
            break;
            case MoveState.landed:
                anim.SetBool("hasLanded", true);
            break;
            case MoveState.falling:
                anim.SetBool("isFalling", true);
            break;
            case MoveState.jumping:
                anim.SetBool("isJumping", true);
            break;
            default:
            break;
        }
    }
}
