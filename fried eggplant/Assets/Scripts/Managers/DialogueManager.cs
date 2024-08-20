using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static DialogueManager GetInstance() { return instance; }
    public static DialogueManager instance;
    private Queue<GameObject> dialogueBoxes = new Queue<GameObject>();
    private GameObject currentBox;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        if (JsonManager.IsSceneInList(SceneManager.GetActiveScene().buildIndex)) DequeueDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBox != null && !currentBox.activeSelf && dialogueBoxes.Count > 0){
            currentBox = dialogueBoxes.Dequeue();
            currentBox.SetActive(true);
        }
    }
    //Queues up dialogue and aborts current dialogue sequence if one already exists
    public void QueueDialog(List<GameObject> list) {
        if (list == null || list.Count == 0){
            return;
        }
        while(dialogueBoxes.Count != 0) {
            dialogueBoxes.Dequeue();
        }
        if(currentBox != null && currentBox.activeSelf) {
            currentBox.SetActive(false);
        }
        foreach (GameObject box in list)
        {
            Debug.Log("queuing up dialogue");
            dialogueBoxes.Enqueue(box);
        }
        currentBox = dialogueBoxes.Dequeue();
        currentBox.SetActive(true);
    }
    //Dequeues all active dialogue
    public void DequeueDialogue() {
        while(dialogueBoxes.Count != 0) {
            dialogueBoxes.Dequeue();
        }
        if(currentBox != null && currentBox.activeSelf) {
            currentBox.SetActive(false);
        }
    }
    public bool isTextBoxActive() {
        return currentBox != null && currentBox.activeSelf;
    }
}
