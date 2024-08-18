using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSequence : MonoBehaviour
{
    [SerializeField] private string nextLevel;
    public GameObject endScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerMovement>().toggleControl(false);
            endScreen.SetActive(true);
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
        
    }
}
