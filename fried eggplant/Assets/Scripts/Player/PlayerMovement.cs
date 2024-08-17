using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum MoveState
    {
        moving,
        idle
    }

    private float speed;
    [Range(1f, 10f)] public float startSpeed = 5;
    [SerializeField] private float maxSpeed = 10;
    public Vector2 lastVelocity;
    private float acceleration = 1.03f;
    private MoveState moveState;
    // Sprite
    private bool isFacingRight = true;
    private float horizontal;

    // Jumping Mechanics
    private bool isJumping;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private float jumpBufferTime = 0.2f;
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
        moveState = MoveState.idle;
        speed = startSpeed;
    }
    void FixedUpdate()
    {
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);
        moveState = (Input.GetAxis("Horizontal") != 0) ? MoveState.moving : MoveState.idle;
        if (moveState == MoveState.moving)
        {
            speed = (speed < maxSpeed) ? speed * acceleration : maxSpeed;
            Debug.Log("Speed: " + speed);
        }
        else
        {
            speed = startSpeed;
        }

        lastVelocity = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Horizontal Axis Input to know which side we are facing
        horizontal = Input.GetAxisRaw("Horizontal");

        CoyoteMechanic();
        Flip();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(DataManager.instance.attemptToBounce()){
            Vector2 bounce = new Vector2(0,0);
            if(collision.relativeVelocity.x > 0){
                //doesn't work cause player is moving...
                bounce.x = lastVelocity.x * -1;
            }
            if(collision.relativeVelocity.y > 0){
                bounce.y = lastVelocity.y * -1;
            }
            rb.velocity += bounce;
        }
        if (collision.gameObject.CompareTag("obstacle"))
        {
            Debug.Log("Collided with wall");
            speed -= 3;
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
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
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
    #endregion
}
