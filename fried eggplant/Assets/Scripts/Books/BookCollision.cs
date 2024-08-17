using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCollision : MonoBehaviour
{
    public float rayLength = 5f; 
    public LayerMask obstacleLayer; 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Update()
    {
        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, rayLength, obstacleLayer);
        RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, rayLength, obstacleLayer);

        if (left.collider != null)
        {
            Debug.Log("GOT HIT AT LEFT");

            OnRaycastHit(left);
        }

        if (right.collider != null)
        {
            Debug.Log("GOT HIT AT RIGHT");

            OnRaycastHit(right);
        }
    }

    void OnRaycastHit(RaycastHit2D hit)
    {
        Debug.Log(hit);
        if (hit.collider.CompareTag("obstacle"))
        {
            rb.isKinematic = false;

            Vector2 forceDirection = ((Vector2)transform.position - hit.point).normalized;
            rb.AddForce(forceDirection * 10f, ForceMode2D.Impulse);
        }
    }
}
