using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isTextBoxActive() {
        return currentBox != null && currentBox.activeSelf;
    }
}
