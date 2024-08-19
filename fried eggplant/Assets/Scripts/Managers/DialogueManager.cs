using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Queue<GameObject> dialogueBoxes = new Queue<GameObject>();
    public List<GameObject> dialogueBoxesList = new List<GameObject>();
    private GameObject currentBox;
    void Start()
    {
        foreach (GameObject box in dialogueBoxesList)
        {
            dialogueBoxes.Enqueue(box);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBox != null && !currentBox.activeSelf && dialogueBoxes.Count > 0){
            currentBox = dialogueBoxes.Dequeue();
            currentBox.SetActive(true);
        }
    }
    void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && dialogueBoxes.Count > 0) {
            if (collision.gameObject.GetComponent<PlayerMovement>().isControllable()) {
                collision.gameObject.GetComponent<PlayerMovement>().toggleControl(false);
                currentBox = dialogueBoxes.Dequeue();
                currentBox.SetActive(true);
            } else if (dialogueBoxes.Count == 0 && !currentBox.activeSelf) {
                collision.gameObject.GetComponent<PlayerMovement>().toggleControl(true);
            }
        } 
    }
}
