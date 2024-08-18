using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    //animation
    private PlayerAnimation playerAnim;
    private PlayerAnimation.MoveState currentMoveState;
    private PlayerAnimation.MoveState animIdle = PlayerAnimation.MoveState.idle;
    private PlayerAnimation.MoveState animRunning = PlayerAnimation.MoveState.running;
    private PlayerAnimation.MoveState animLanded = PlayerAnimation.MoveState.landed;
    private PlayerAnimation.MoveState animFalling = PlayerAnimation.MoveState.falling;
    private PlayerAnimation.MoveState animJumping = PlayerAnimation.MoveState.jumping;
    private PlayerAnimation.MoveState animHasBook = PlayerAnimation.MoveState.hasBook;
    private PlayerAnimation.MoveState animHasTurned = PlayerAnimation.MoveState.hasTurned;
    [SerializeField] private float speed;
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
    //private bool isFlipping = false;
    private BookCollector bookCollector;
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

    // Ground Checks
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Components
    public Rigidbody2D rb;

    //Movement Variables
    [SerializeField] public float C;
    [SerializeField] public float c;
    [SerializeField] public float a;
    [SerializeField] public float h;
    [SerializeField] public float d;
    private const float pi = Mathf.PI;
    private float x;


    // Start is called before the first frame update
    void Start()
    {
        currentMoveState = animIdle;
        //animator = GetComponent<Animator>();
        playerAnim = GetComponent<PlayerAnimation>();
        bookCollector = GetComponent<BookCollector>();
        introSplash = GameObject.FindGameObjectWithTag("Intro Splash");
        canMove = introSplash == null;
        hasStarted = canMove;
        isDead = false;
    }

    void FixedUpdate()
    {
        if (canMove && !isDashing && !isBouncing){
            transform.Translate(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0);
            currentMoveState = (Input.GetAxisRaw("Horizontal") != 0) ? animRunning : animIdle;
            Move();
        }
        if(introSplash != null){
            if (introSplash.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("End") && !hasStarted) {
                canMove = true;
                hasStarted = true;
            } 
        }
        if(DataManager.instance.instantBoostAmount != new Vector2(0,0)){
            boost = DataManager.instance.applyBoost() * boostForce;
            if(boost.x != 0){
                StartCoroutine(dash());
            }
            if(boost.y != 0){
                StartCoroutine(lift());
            }
        }
    }

    void Move()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (IsGrounded()) {
                speed = C * Mathf.Atan(c * x - a) + (pi / d) + h;
                x += Time.deltaTime;
            } 
            timeIdle = 0;
        }
        else 
        {
            timeIdle += Time.deltaTime;
            if (timeIdle >= 0.5f) {
                x = 0;
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !isDashing && !isBouncing) {
            // Get Horizontal Axis Input to know which side we are facing
            horizontal = Input.GetAxisRaw("Horizontal");
            hasBook = bookCollector.getNumBooks() > 0;
            CoyoteMechanic();
        }
        //PlayAnimation();
        //animationUpdate()
        if (rb.velocity.y > 0.1f) {
            currentMoveState = animJumping;
        } else if (rb.velocity.y < -0.1f) {
            currentMoveState = animFalling;
        }   
        if (IsGrounded() && !hasLanded) {
            currentMoveState = animLanded;
            hasLanded = true;
        } else if (!IsGrounded()) {
            hasLanded = false;
            isBouncing = false;
        }
        Flip();
        playerAnim.AnimationUpdate(currentMoveState, hasBook);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("obstacle") && !isCooldown)
        {
            Debug.Log("Collided with wall");
            x = 0;
            collisionCooldown();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(DataManager.instance.canBounce){
            rb.AddForce(collision.contacts[0].normal * bounceForce);
            if(collision.relativeVelocity.y < 0){
                rb.AddForce(Vector2.up * bounceForce);
            }
            if(IsGrounded()){
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
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && canMove)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f && canMove)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
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
            x = 0;
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
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
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
        var temp = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(boost.x*(isFacingRight?1:-1),0));
        isDashing = true;
        yield return new WaitForSeconds(boostTime);
        rb.velocity = new Vector2(0,0);
        rb.gravityScale = temp;
        isDashing = false;
    }

    IEnumerator lift(){
        var temp = rb.gravityScale;
        rb.gravityScale = 0;
        rb.AddForce(new Vector2(0,boost.y/2));
        yield return new WaitForSeconds(boostTime);
        rb.velocity = new Vector2(0,0);
        rb.gravityScale = temp;
    }

    IEnumerator waitForBounceCooldown(){
        isBouncing = true;
        yield return new WaitForSeconds(bounceCooldown);
        isBouncing = false;
    }
}

