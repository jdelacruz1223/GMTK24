using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{

    [HideInInspector] public Vector2 velocity;
    [HideInInspector] public new Rigidbody2D rigidbody { get; private set; }
    // protected ContactFilter2D contactFilter;
    private bool isGrounded = false;
    public bool IsGrounded { get => isGrounded; }
    private readonly List<RaycastHit2D> hitList = new(16);
    public float shellSize = 1 / 32;

    void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Init contact filter
        // contactFilter = new ContactFilter2D
        // {
        //     useTriggers = false,
        //     layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer),
        //     useLayerMask = true,
        // };
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
            if (Mathf.Abs(move.x) > float.Epsilon)
            {
                var moveX = new Vector2(move.x, 0);
                var hit = CollisionCheck(moveX);
                if (hit)
                {
                    // Hit wall
                    rigidbody.position += moveX.normalized * (hit.distance - shellSize);
                    velocity.x = 0;
                }
                else
                {
                    rigidbody.position += moveX;
                }
            }

            // MoveY
            if (Mathf.Abs(move.y) > float.Epsilon)
            {
                var moveY = new Vector2(0, move.y);
                var hit = CollisionCheck(moveY);
                if (hit)
                {
                    // Hit floor/ceiling
                    rigidbody.position += moveY.normalized * (hit.distance - shellSize);
                    velocity.y = 0;
                    if (move.y < 0)
                    {
                        isGrounded = true;
                    }
                }
                else
                {
                    rigidbody.position += moveY;
                }
            }
        }
    }

    RaycastHit2D CollisionCheck(Vector2 amount)
    {
        var filter = new ContactFilter2D {
            layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer),
            useLayerMask = true,
            useTriggers = false,
        };
        hitList.Clear();
        rigidbody.Cast(amount, filter, hitList, amount.magnitude + shellSize);
        if (hitList.Count == 0) return default;
        return hitList
            .Where(hit => {
                if (!hit.collider.gameObject.TryGetComponent<PlatformEffector2D>(out var platform)) return true;
                if (platform.useOneWay == false) return true;
                // Don't count it if it's inside the player
                if (hit.distance == 0) return false;
                return Vector2.Dot(amount, platform.transform.up) < 0;
            })
            .OrderBy(h => h.distance)
            .FirstOrDefault();
    }
}