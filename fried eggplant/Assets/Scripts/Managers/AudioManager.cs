using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager GetInstance() { return instance; }
    public static AudioManager instance;
    private AudioSource audioSource;
    private float normalVolume;
    public AudioClip menuMusic;
    public AudioClip introBGM;
    public AudioClip BGMLoop;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        normalVolume = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenuScene" && audioSource.clip != menuMusic) {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        } else if(SceneManager.GetActiveScene().name != "MainMenuScene" && audioSource.clip != introBGM && audioSource.clip != BGMLoop) {
            audioSource.Stop();
            audioSource.clip = introBGM;
            audioSource.loop = false;
            audioSource.Play();
        } else if(SceneManager.GetActiveScene().name != "MainMenuScene" && audioSource.clip == introBGM && !audioSource.isPlaying) {
            audioSource.clip = BGMLoop;
            audioSource.loop = true;
            audioSource.Play();
        }
        if(GameObject.Find("DimOverlay(Clone)")) {
            audioSource.volume = 0.025f;
        } else {
            audioSource.volume = normalVolume;
        }
    }
}
