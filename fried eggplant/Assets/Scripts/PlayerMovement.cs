using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum MoveState {
        moving,
        idle
    }
    [Range(1f,10f)] public float startSpeed = 5;
    private float speed;
    [SerializeField] private float maxSpeed = 10;
    private float acceleration = 1.03f;
    public Rigidbody2D rb;
    private MoveState moveState;
    // Start is called before the first frame update
    void Start()
    {
        moveState = MoveState.idle;
        speed = startSpeed;
    }
    void FixedUpdate() {
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);
        moveState = (Input.GetAxis("Horizontal") != 0) ? MoveState.moving : MoveState.idle;
        if (moveState == MoveState.moving) {
            speed = (speed < maxSpeed) ? speed * acceleration : maxSpeed;
            Debug.Log("Speed: " + speed);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("obstacle")) {
            Debug.Log("Collided with wall");
            speed -= 3;
        }
    }
}
