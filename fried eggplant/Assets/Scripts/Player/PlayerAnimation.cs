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
        if (!book) anim.SetBool("hasBook", false);

        switch (moveState)
        {
            case MoveState.idle:
                anim.SetBool("isIdling",true);
                anim.SetBool("isRunning",false);
                anim.SetBool("isFalling",false);
                anim.SetBool("hasLanded", false);
            break;
            case MoveState.running:
                anim.SetBool("isRunning",true);
                anim.SetBool("isIdling",false);
            break;
            case MoveState.landed:
                anim.SetTrigger("hasLanded");
            break;
            case MoveState.falling:
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping",false);
            break;
            case MoveState.jumping:
                anim.SetBool("isJumping", true);
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
