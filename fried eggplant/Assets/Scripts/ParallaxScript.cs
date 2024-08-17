using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    [Range(1f,10f)] public float distanceModifier = 2f; //Controls how fast each layer moves
    public GameObject foreground;
    public GameObject midground;
    public GameObject background;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            return;
        }
        if (background != null) {
            background.transform.position = new Vector3(player.transform.position.x * -1/distanceModifier, player.transform.position.y * -1/distanceModifier, 1);
        } 
        if (midground != null) {
            midground.transform.position = new Vector3(player.transform.position.x * -1/(distanceModifier * 1.25f), player.transform.position.y * -1/(distanceModifier * 1.25f), 2);
        }
        if (foreground != null) {
            foreground.transform.position = new Vector3(player.transform.position.x * -1/(distanceModifier * 1.5f), player.transform.position.y * -1/(distanceModifier * 1.5f), 3);
        }
    }
}
