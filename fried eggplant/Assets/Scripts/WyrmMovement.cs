using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WyrmMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    private int spawnPointIndex = 0;
    [SerializeField] private float speed;
    private bool isFollowing = false;
    [SerializeField] private GameObject playerFollow;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        if (spawnPoints.Count == 0) {
            gameObject.SetActive(false);
        } else {
            transform.position = spawnPoints[spawnPointIndex].transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(isFollowing) {
            transform.position = Vector2.MoveTowards(transform.position, playerFollow.transform.position, speed);
        }
    }
    void Update()
    {
        if (spawnPointIndex >= spawnPoints.Count && !DialogueManager.instance.isTextBoxActive()) {
            gameObject.SetActive(false);
        }

    }
    public void startFollowing() {
        if (!isFollowing) {
            StartCoroutine(FollowPlayer());
            isFollowing = true;
        }
        spawnPointIndex++;
    }
    IEnumerator FollowPlayer() {
        while(DialogueManager.instance.isTextBoxActive()) {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        isFollowing = false;
        if(spawnPointIndex < spawnPoints.Count) transform.position = spawnPoints[spawnPointIndex].transform.position;
    }
}
