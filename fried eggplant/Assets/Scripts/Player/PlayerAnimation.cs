using System.Reactive.Concurrency;
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
    [HideInInspector] public Animator anim;
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private ParticleSystem dash;
    [SerializeField] private ParticleSystem lift;
    [SerializeField] private ParticleSystem slam;
    public bool isDashing;
    public bool isLifting;
    public bool isSlamming;
    private float dustCooldown = 0.01f;
    [SerializeField] private float currentCooldown = 0f;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update(){
        if(isDashing){
            dash.Emit(1);
        }
        else{
            dash.Stop();
        }
        if(isLifting){
            lift.Emit(1);
        }
        else{
            lift.Stop();
        }
        if(isSlamming){
            slam.Play();
        }
        else{
            slam.Stop();
        }
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
                if (dust != null && GetComponent<PlayerMovement>().IsGrounded){
                    if(currentCooldown >= dustCooldown){
                        dust.Emit(1);
                        dust.Play();
                        currentCooldown = 0;   
                    }
                    else{
                        currentCooldown += Time.deltaTime;
                    }
                }
            break;
            case MoveState.landed:
                anim.SetTrigger("hasLanded");
                if (dust != null){
                    dust.Emit(10);
                    dust.Play();
                }
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
