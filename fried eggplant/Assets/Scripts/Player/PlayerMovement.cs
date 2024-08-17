using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float speed;
    [Range(1f, 10f)] public float startSpeed = 5;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float acceleration = 1.03f;
    [SerializeField] private float cooldown = 1f;
    private bool isCooldown = false;
    private bool hasBook;
    private BookCollector bookCollector;
    
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


    // Start is called before the first frame update
    void Start()
    {
        currentMoveState = animIdle;
        //animator = GetComponent<Animator>();
        speed = startSpeed;
        playerAnim = GetComponent<PlayerAnimation>();
        bookCollector = GetComponent<BookCollector>();
    }
    void FixedUpdate()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0);
        currentMoveState = (Input.GetAxisRaw("Horizontal") != 0) ? animRunning : animIdle;
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            speed = (speed < maxSpeed) ? speed * acceleration : maxSpeed;
        //     isRunning = true;
        //     isIdling = false;
        }
        else
        {
            speed = startSpeed;
        //     isRunning = false;

        //     if (!IsGrounded()) isIdling = false; else isIdling = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get Horizontal Axis Input to know which side we are facing
        horizontal = Input.GetAxisRaw("Horizontal");
        hasBook = (bookCollector.getNumBooks() > 0) ? true : false;
        CoyoteMechanic();
        //PlayAnimation();
        //animationUpdate()
        if (rb.velocity.y > 0 ) {
            currentMoveState = animJumping;
        } else if (rb.velocity.y < 0) {
            currentMoveState = animFalling;
        }   
        if (IsGrounded() && !hasLanded) {
            currentMoveState = animLanded;
            hasLanded = true;
        } else if (!IsGrounded()) {
            hasLanded = false;
        }
        playerAnim.AnimationUpdate(currentMoveState, hasBook);
        Flip();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("obstacle") && !isCooldown)
        {
            Debug.Log("Collided with wall");
            speed = (speed > startSpeed) ? speed - 3 : startSpeed;
            collisionCooldown();
        }
    }

    //     isJumping = Input.GetButtonDown("Jump") && IsGrounded();
    //     isFalling = !IsGrounded() && rb.velocity.y > 0;


 
    // }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("platform") && transform.position.y > collision.transform.position.y) {
            transform.parent = collision.transform;
        }
    }
    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("platform")) {
            transform.parent = null;
        }
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

        if (Input.GetButtonDown("Jump"))
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

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
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
            //flipDelay();
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            speed = startSpeed;
        }
    }
    /*private IEnumerator flipDelay(){
        currentMoveState = animHasTurned;
        yield return new WaitForSeconds(5/12);
        currentMoveState = animIdle;
    }*/
    #endregion

    #region Checks
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
}

