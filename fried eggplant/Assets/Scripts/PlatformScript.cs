using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [SerializeField] private bool isMovingPlatform = false; 
    [SerializeField] private bool isOneWay = false;
    [SerializeField] private bool bounce = false; // Goes back and forth if true
    [SerializeField] private float speed = 5f;
    private Vector2 direction;
    private SpriteRenderer sprite;
    private Transform player;
    private Collider2D collision;
    private int wpIndex = 0;
    private int bounceControl = 1; 
    public Transform[] waypoints; // The platform will be set to the first waypoint's position

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints != null) {
            transform.position = (waypoints[0] != null) ? waypoints[0].position : transform.position;
        }
        sprite = GetComponent<SpriteRenderer>();
        collision = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMovingPlatform) {
            if(waypoints == null) {
                Debug.LogError("No waypoints set; platform cannot move");
                return;
            }
            direction = waypoints[wpIndex].position - transform.position;
            if (direction.magnitude > 0.05f) {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[wpIndex].position, speed * Time.deltaTime);
            } else if (!bounce) {
                wpIndex = (wpIndex + 1 < waypoints.Length) ? wpIndex + 1 : 0;
            } else {
                wpIndex += bounceControl;
                if (wpIndex + 1 >= waypoints.Length) {
                    bounceControl = -1;
                } else if (wpIndex == 0) {
                    bounceControl = 1;
                }
            }
        }
        if(isOneWay) {
            
        }
    }
}
