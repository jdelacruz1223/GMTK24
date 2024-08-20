using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public List<string> dialogue = new List<string>();
    private Queue<string> dialogueQueue = new Queue<string>();
    public bool forreSpeak; //if true, forre is the speaker, else wyrm is the speaker
    public TMP_Text dialogueText;
    public TMP_Text speakerText;
    public Image currentPortrait;
    public Sprite forrePotrait;
    public Sprite wyrmPortrait;
    [SerializeField] private float textSpeed = 1f;
    [SerializeField] private float hangTime = 1f;
    [SerializeField] private AudioSource speech;
    [SerializeField] private AudioClip forreSpeech;
    [SerializeField] private AudioClip wyrmSpeech;
    public List<string> noSoundChar = new List<string>();


    //private float basePitch;
    
    
    void Awake() {
        
        dialogueText.text = "";
        speakerText.text = forreSpeak ? "Forre" : "Wyrm";
        currentPortrait.sprite = forreSpeak ? forrePotrait : wyrmPortrait;
        foreach (string d in dialogue) {dialogueQueue.Enqueue(d);}
        StartCoroutine(displayText());
    }
    void Start() {
        
        //basePitch = speech.pitch;
    }
    void Update() {
    
    }
    
    IEnumerator displayText() {
        yield return new WaitForSeconds(0.1f);
        string d = dialogueQueue.Dequeue();
        int i = 0;
        while (i < d.Length) {
            string c = d.Substring(i,1);
            if (c.CompareTo("/") == 0 && (i + 1) != d.Length)  {
                c = d.Substring(i,2);
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
                if (c.Length > 1) c = d.Substring(i,1);
                if (!noSoundChar.Contains(c) && speech.clip != null) {
                    //speech.pitch *= Random.value + basePitch;
                    speech.Play();
                    //speech.pitch = basePitch;
                } 
                dialogueText.text += c;
                i++;
                break;
            }
            yield return new WaitForSeconds(1 / (textSpeed * 10));
        }
        yield return new WaitForSeconds(hangTime);
        if (dialogueQueue.Count > 0) {
            StartCoroutine(displayText());
            switchSpeakers();
        } else {
            gameObject.SetActive(false);
        }
    }
    private void switchSpeakers() {
        dialogueText.text = "";
        if (forreSpeak) {
            forreSpeak = false;
            speakerText.text = "Wyrm";
            currentPortrait.sprite = wyrmPortrait;
            speech.clip = wyrmSpeech;
        } else {
            forreSpeak = true;
            speakerText.text = "Forre";
            currentPortrait.sprite = forrePotrait;
            speech.clip = forreSpeech;
        }
        
    }
}
