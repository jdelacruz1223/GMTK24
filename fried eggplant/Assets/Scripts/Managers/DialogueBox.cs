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
        int i = 0;
        while (i < dialogue.Length) {
            string c = dialogue.Substring(i,1);
            if (c.CompareTo("/") == 0 && (i + 1) != dialogue.Length)  {
                c = dialogue.Substring(i,2);
            }
            switch (c) {
                case "/n":
                dialogueText.text += "\n";
                i += 2;
                break;
                case "/.":
                i+= 2;
                yield return new WaitForSeconds(0.25f);
                break;
                case "/|":
                i+= 2;
                yield return new WaitForSeconds(1f);
                break;
                default:
                if (c.Length > 1) c = dialogue.Substring(i,1);
                dialogueText.text += c;
                i++;
                break;
            }
            yield return new WaitForSeconds(1 / (textSpeed * 10));
        }
        /*
        foreach(char c in dialogue) {
            dialogueText.text += c;
            yield return new WaitForSeconds(1 / (textSpeed * 10));
        }
        */
        yield return new WaitForSeconds(hangTime);
        gameObject.SetActive(false);
    }
    
}
