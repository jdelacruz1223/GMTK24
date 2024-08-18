using System.Collections;
using Cinemachine;
using UnityEngine;

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
    [Range(1f, 10f)] public float startSpeed = 5;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float acceleration = 1.03f;
    [SerializeField] private float cooldown = 1f;
    private bool isCooldown = false;
    private bool hasBook;
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
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private Vector2 groundCheckSize;

    // Components
    // public Rigidbody2D rb;

    [SerializeField] private new CinemachineVirtualCamera camera;
    [SerializeField] private AnimationCurve cameraZoomCurve;


    // Start is called before the first frame update
    void Start()
    {
        currentMoveState = animIdle;
        introSplash = GameObject.FindGameObjectWithTag("Intro Splash");
        canMove = introSplash == null;
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

        var accel = 0.3f;
        var targetSpeed = 0f;

        if (Mathf.Abs(horizontal) > float.Epsilon) {
            if (Mathf.Abs(speed) > startSpeed) accel = 0.05f;
            targetSpeed = maxSpeed * Mathf.Sign(horizontal);
        }
        speed = Mathf.MoveTowards(speed, targetSpeed, accel);

        velocity.x = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) {
            // 
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
        }

        Flip();
        playerAnimation.AnimationUpdate(currentMoveState, hasBook);
    }

    // private void OnCollisionStay2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("obstacle") && !isCooldown)
    //     {
    //         Debug.Log("Collided with wall");
    //         speed = (speed > startSpeed) ? speed - 3 : startSpeed;
    //         collisionCooldown();
    //     }
    // }

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
            //flipDelay();
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    /*private IEnumerator flipDelay(){
        currentMoveState = animHasTurned;
        yield return new WaitForSeconds(5/12);
        currentMoveState = animIdle;
    }*/
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

