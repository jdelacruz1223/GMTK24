using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneSequence : MonoBehaviour
{
    public string nextLevel;
    public GameObject endScreen;
    public AudioSource jingle;
    private GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (collision.gameObject.GetComponent<PlayerMovement>().isControllable()) {
                collision.gameObject.GetComponent<PlayerMovement>().toggleControl(false);
                Instantiate(endScreen, new Vector3(cam.transform.position.x, cam.transform.position.y, 0), Quaternion.identity, cam.transform);
                TimeManager.instance.endLevel();
                jingle.Play();
            }
        }
    }
    public void GoToMainMenu() {
        SceneHandler.GotoScene("MainMenuScene", hasTransition: true);
    }
    [ContextMenu("Go To Next Level")]
    public void GoToNextLevel() {
        SceneHandler.GotoScene(nextLevel, hasTransition: true);
    }
    public void RetryLevel() {
        SceneHandler.GotoScene(SceneManager.GetActiveScene().name);
    }
}
