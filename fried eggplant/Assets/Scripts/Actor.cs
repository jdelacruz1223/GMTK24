using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{

    [HideInInspector] public Vector2 velocity;
    protected new Rigidbody2D rigidbody;
    protected ContactFilter2D contactFilter;
    private bool isGrounded = false;
    protected bool IsGrounded { get => isGrounded; }
    private readonly List<RaycastHit2D> hitList = new(16);
    public float shellSize = 1/32;

    void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Init contact filter
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void FixedUpdate()
    {
        velocity += rigidbody.gravityScale * Time.deltaTime * Physics2D.gravity;
        ActorFixedUpdate();
        Move(velocity * Time.deltaTime);
    }

    abstract protected void ActorFixedUpdate();

    void Move(Vector2 move)
    {
        if (Mathf.Abs(move.y) > float.Epsilon) isGrounded = false;
        if (move.magnitude > float.Epsilon)
        {
            // MoveX
            if (Mathf.Abs(move.x) > float.Epsilon) {
                var moveX = new Vector2(move.x, 0);
                var hit = CollisionCheck(moveX);
                if (hit == null) {
                    rigidbody.position += moveX;
                } else {
                    // Hit wall
                    Debug.Log("Hit wall");
                    rigidbody.position += moveX.normalized * (hit.Value.distance - shellSize);
                    velocity.x = 0;
                }
            }

            // MoveY
            if (Mathf.Abs(move.y) > float.Epsilon) {
                var moveY = new Vector2(0, move.y);
                var hit = CollisionCheck(moveY);
                if (hit == null) {
                    rigidbody.position += moveY;
                } else {
                    // Hit floor/ceiling
                    rigidbody.position += moveY.normalized * (hit.Value.distance - shellSize);
                    velocity.y = 0;
                    if (move.y < 0) {
                        isGrounded = true;
                    }
                }
            }
        }
    }

    RaycastHit2D? CollisionCheck(Vector2 amount) {
        int count = rigidbody.Cast(amount, contactFilter, hitList, amount.magnitude + shellSize);
        if (count == 1) return hitList.First();
        if (count > 1) return hitList.OrderBy(h => h.distance).First();
        return null;
    }
}