using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public string dialogue;
    public string speaker;
    public TMP_Text dialogueText;
    public TMP_Text speakerText;
    [SerializeField] private float textSpeed = 1f;
    [SerializeField] private float hangTime = 1f;
    
    
    void Awake() {
        StartCoroutine(displayText());
    }
    void Start() {
        speakerText.text = speaker;
    }
    void Update() {

    }
    
    IEnumerator displayText() {
        foreach(char c in dialogue) {
            dialogueText.text += c;
            yield return new WaitForSeconds(1 / (textSpeed * 10));
        }
        yield return new WaitForSeconds(hangTime);
        gameObject.SetActive(false);
    }
    
}
