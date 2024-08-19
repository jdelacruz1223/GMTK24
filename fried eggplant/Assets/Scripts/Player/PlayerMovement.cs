using System;
using System.Collections;
using Cinemachine;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : Actor
{
    //animation
    [SerializeField] private PlayerAnimation playerAnimation;
    private PlayerAnimation.MoveState currentMoveState;
    private readonly PlayerAnimation.MoveState animIdle = PlayerAnimation.MoveState.idle;
    private readonly PlayerAnimation.MoveState animRunning = PlayerAnimation.MoveState.running;
    private readonly PlayerAnimation.MoveState animLanded = PlayerAnimation.MoveState.landed;
    private readonly PlayerAnimation.MoveState animFalling = PlayerAnimation.MoveState.falling;
    private readonly PlayerAnimation.MoveState animJumping = PlayerAnimation.MoveState.jumping;
    private readonly PlayerAnimation.MoveState animHasBook = PlayerAnimation.MoveState.hasBook;
    private readonly PlayerAnimation.MoveState animHasTurned = PlayerAnimation.MoveState.hasTurned;
    private bool canMove;
    private float timeIdle;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private float boostTime = 1f;
    [SerializeField] private float bounceCooldown = 1f;
    private Vector2 boost;
    [SerializeField] private float bounceForce = 100f;
    [SerializeField] private float boostForce = 100f;
    private bool isDashing = false;
    private bool isBouncing = false;
    private bool isCooldown = false;
    private bool hasBook;
    private bool hasStarted;
    private bool isDead;
    [SerializeField] private BookCollector bookCollector;
    private GameObject introSplash;
    
    // Sprite
    private bool isFacingRight = true;
    private float horizontal;
    private bool hasLanded;

    // Jumping Mechanics
    private bool isJumping;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private float jumpBufferTime = 0.5f;
    private float jumpBufferCounter;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private bool killJump;

    // Ground Checks
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private Vector2 groundCheckSize;

    // Components
    // public Rigidbody2D rb;

    [SerializeField] private new CinemachineVirtualCamera camera;
    [SerializeField] private AnimationCurve cameraZoomCurve;

    //Movement Variables
    [Header("Movement Config")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float startSpeed;

    [Tooltip("How quickly the player goes from 0 to startSpeed")]
    [SerializeField] private float moveAccel = 0.5f;

    [Tooltip("Acceleration for going from startSpeed to maxSpeed")]
    [SerializeField] private float momentumAccel = 0.1f;
    
    [Tooltip("How quickly the player turns around")]
    [SerializeField] private float skidAccel = 0.8f;
    
    [Tooltip("How quickly the player stops with no buttons pressed")]
    [SerializeField] private float stopAccel = 0.7f;
    
    [Tooltip("Scales the current accel when in the air")]
    [SerializeField] private float airMultiplier = 0.65f;

    // Start is called before the first frame update
    void Start()
    {
        currentMoveState = animIdle;
        introSplash = GameObject.FindGameObjectWithTag("Intro Splash");
        canMove = introSplash == null;
        hasStarted = canMove;
        isDead = false;
        camera = FindFirstObjectByType<CinemachineVirtualCamera>();
    }
    
    override
    protected void ActorFixedUpdate()
    {
        FixedUpdateMovement();
        if (playerAnimation.anim.GetCurrentAnimatorStateInfo(0).IsName("End")) canMove = true;
        camera.m_Lens.OrthographicSize = 4 + cameraZoomCurve.Evaluate(Mathf.Abs(velocity.x) / maxSpeed);
    }

    void FixedUpdateMovement() {
        if (!canMove) return;

        // fixed update Jump handling, cannot get button presses in here!
        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0) {
            velocity.y = jumpingPower;
            jumpBufferCounter = 0;
        }

        if (killJump) {
            velocity.y *= 0.5f;
            killJump = false;
            coyoteTimeCounter = 0;
        }

        
        var speed = velocity.x;
        var horizontal = Input.GetAxisRaw("Horizontal");

        var accel = stopAccel;
        
        var targetSpeed = 0f;

        if (Mathf.Abs(horizontal) > float.Epsilon) {
            accel = moveAccel;
            if (Mathf.Sign(velocity.x) == -Mathf.Sign(horizontal)) accel = skidAccel;
            else if (Mathf.Abs(speed) > startSpeed) accel = momentumAccel;
            targetSpeed = maxSpeed * Mathf.Sign(horizontal);
        }
        if (!IsGrounded) accel *= airMultiplier;
        speed = Mathf.MoveTowards(speed, targetSpeed, accel);

        velocity.x = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !isDashing && !isBouncing) {
            CoyoteMechanic();
            // Get Horizontal Axis Input to know which side we are facing
            horizontal = Input.GetAxisRaw("Horizontal");
            hasBook = bookCollector.getNumBooks() > 0;
            currentMoveState = horizontal != 0 ? animRunning : animIdle;
        }
        //PlayAnimation();
        //animationUpdate()
        if (velocity.y > 0.1f) {
            currentMoveState = animJumping;
        } else if (velocity.y < -0.1f) {
            currentMoveState = animFalling;
        }   
        if (IsGrounded && !hasLanded) {
            currentMoveState = animLanded;
            hasLanded = true;
        } else if (!IsGrounded) {
            hasLanded = false;
            isBouncing = false;
        }
        Flip();
        playerAnimation.AnimationUpdate(currentMoveState, hasBook);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("obstacle") && !isCooldown)
        {
            Debug.Log("Collided with wall");
            collisionCooldown();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (DataManager.instance.canBounce) {
            velocity += collision.contacts[0].normal * bounceForce;
            if (collision.relativeVelocity.y < 0) {
                velocity += Vector2.up * bounceForce;
            }
            if (IsGrounded) {
                DataManager.instance.canBounce = false;
            }
        }
        if (collision.gameObject.CompareTag("platform") && transform.position.y > collision.transform.position.y) {
            transform.parent = collision.transform;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("platform")) {
            transform.parent = null;
        }
    }

    //Toggles player control on or off
    public void toggleControl() {
        canMove = !canMove;
    }

    //Sets player control to set boolean value
    public void toggleControl(bool t) {
        canMove = t;
    }
    
    #region Jumping Mechanics
    private void CoyoteMechanic()
    {
        if (IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump was pressed");
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump") && velocity.y > 0f)
        {
            killJump = true;
        }
    }
    #endregion

    #region Sprites
    /// <summary>
    /// Flips the Sprite Left and Right depending on the Horizontal Input
    /// </summary>
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            //currentMoveState = animHasTurned;
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    /*
    private IEnumerator flipDelay(){
        isFlipping = true;
        currentMoveState = animHasTurned;
        yield return new WaitForSeconds(5/12);
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        currentMoveState = animIdle;
        isFlipping = false;
    }
    */
    #endregion

    #region Checks
    public bool isPlayerDead() {
        return isDead;
    }
    #endregion


    #region Cooldowns
    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    IEnumerator collisionCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
    #endregion

    IEnumerator dash(){
        var temp = rigidbody.gravityScale;
        rigidbody.gravityScale = 0;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        rigidbody.AddForce(new Vector2(boost.x*(isFacingRight?1:-1),0));
        isDashing = true;
        yield return new WaitForSeconds(boostTime);
        rigidbody.velocity = new Vector2(0,0);
        rigidbody.gravityScale = temp;
        isDashing = false;
    }

    IEnumerator lift(){
        var temp = rigidbody.gravityScale;
        rigidbody.gravityScale = 0;
        rigidbody.AddForce(new Vector2(0,boost.y/2));
        yield return new WaitForSeconds(boostTime);
        rigidbody.velocity = new Vector2(0,0);
        rigidbody.gravityScale = temp;
    }

    IEnumerator waitForBounceCooldown() {
        isBouncing = true;
        yield return new WaitForSeconds(bounceCooldown);
        isBouncing = false;
    }
}

