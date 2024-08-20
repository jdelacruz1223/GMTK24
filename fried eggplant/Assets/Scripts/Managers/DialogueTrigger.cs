using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<GameObject> dialogueBoxes = new List<GameObject>();
    private GameObject wyrm;
    // Start is called before the first frame update
    void Start()
    {
        wyrm = GameObject.FindGameObjectWithTag("Wyrm");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && dialogueBoxes.Count > 0 && (dialogueBoxes == null || dialogueBoxes.Count > 0)) {
            DialogueManager.instance.QueueDialog(dialogueBoxes);
            wyrm.GetComponent<WyrmMovement>().startFollowing();
            while(dialogueBoxes.Count > 0) {
                dialogueBoxes.Remove(dialogueBoxes[0]);
            }
        }
    }
}
