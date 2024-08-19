using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneSequence : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;
    public AudioSource jingle;
    private GameObject cam;
    [SerializeField] private GameObject dimOverlay;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        if(endScreen.activeSelf) endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        endScreen.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (collision.gameObject.GetComponent<PlayerMovement>().isControllable()) {
                collision.gameObject.GetComponent<PlayerMovement>().toggleControl(false);
                endScreen.SetActive(true);
                Instantiate(dimOverlay, new Vector3(cam.transform.position.x, cam.transform.position.y, 0), Quaternion.identity, cam.transform);
                TimeManager.instance.endLevel();
                jingle.Play();
            }
        }
    }
    
}
