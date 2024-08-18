using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GetInstance() { return instance; }
    public static GameManager instance;

    public string nextScene { get; private set; }
    public float currentTime { get; private set; }

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

    async void Start()
    {
        // Initialize Supabase
        await SupabaseClient.GetInstance().InitializeSupabase();

        nextScene = "";
    }

    public void setNextScene(string name) => nextScene = name;
    public void setCurrentTime(float time) => currentTime = time;
}
