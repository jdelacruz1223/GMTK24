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
    private float timeIdle;
    [SerializeField] private float cooldown = 1f;

    [Header("Bounce Config")]
    [SerializeField] private float bounceCooldown = 1f;
    [SerializeField] private float bounceForce = 100f;
    private bool isBouncing = false;
    
    [Header("Dash Config")]
    [SerializeField] private float dashTime = 1f;
    [SerializeField] private float dashForce = 100f;
    private float dashCounter;
    private Vector2 dash;
    private bool IsDashing { get => dashCounter > 0; }

    private bool isCooldown = false;
    private bool hasBook;
    private bool isDead;
    [SerializeField] private BookCollector bookCollector;
    private GameObject introSplash;
    
    // Sprite
    private bool isFacingRight = true;
    private float horizontal;
    private bool hasLanded;

    // Jumping Mechanics
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

    [Header("Camera Config")]
    [SerializeField] private AnimationCurve cameraZoomCurve;
    [SerializeField] private Transform cameraTarget;
    private new CinemachineVirtualCamera camera;
    private float initialCameraSize;
    private Vector3 initialCameraTargetOffset;

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
        isDead = false;
        camera = FindFirstObjectByType<CinemachineVirtualCamera>();
        initialCameraSize = camera.m_Lens.OrthographicSize;
        initialCameraTargetOffset = cameraTarget.localPosition;
    }
    
    override
    protected void ActorFixedUpdate()
    {
        FixedUpdateMovement();

        if (playerAnimation.anim.GetCurrentAnimatorStateInfo(0).IsName("End")) canMove = true;
        var currentSize = camera.m_Lens.OrthographicSize;
        var targetSize = initialCameraSize + cameraZoomCurve.Evaluate(Mathf.Abs(velocity.x) / maxSpeed);
        camera.m_Lens.OrthographicSize = Mathf.MoveTowards(currentSize, targetSize, 0.05f);

        var offsetScale = Mathf.Min(1f, Mathf.Abs(velocity.x) / maxSpeed) * Mathf.Sign(velocity.x);
//        Debug.Log($"offsetScale: {offsetScale * initialCameraTargetOffset.x}");
        var offset = new Vector3(offsetScale * initialCameraTargetOffset.x, initialCameraTargetOffset.y, initialCameraTargetOffset.z);
        cameraTarget.localPosition = transform.position + offset;
    }

    void FixedUpdateMovement() {

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
        var horizontal = Input.GetAxisRaw("Horizontal") * (canMove ? 1 : 0);

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

        // Dash overwrite
        if (DataManager.instance.instantBoostAmount.sqrMagnitude > float.Epsilon) {
            dashCounter = dashTime;
            dash = DataManager.instance.applyBoost() * dashForce;
        }

        if (dashCounter > 0) {
            dashCounter -= Time.deltaTime;
            velocity.y = 0;
            velocity.x = dash.x * (isFacingRight ? 1 : -1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !IsDashing && !isBouncing) {
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
            if (IsGrounded) currentMoveState = animHasTurned;
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    #endregion

    #region Checks
    public bool isPlayerDead() {
        return isDead;
    }
    public bool isControllable() {
        return canMove;
    }
    #endregion


    #region Cooldowns

    IEnumerator collisionCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
    #endregion

    IEnumerator waitForBounceCooldown() {
        isBouncing = true;
        yield return new WaitForSeconds(bounceCooldown);
        isBouncing = false;
    }


}

