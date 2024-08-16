using Supabase.Interfaces;
using Supabase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupabaseClient : MonoBehaviour
{
    private static SupabaseClient _instance;
    public static SupabaseClient Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObj = new GameObject("SupabaseClient");
                _instance = singletonObj.AddComponent<SupabaseClient>();
                DontDestroyOnLoad(singletonObj);
            }
            return _instance;
        }
    }

    private Client _supabase;
    public Client supabase => _supabase;

    private void Awake()
    {
        if (_instance != null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSupabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void InitializeSupabase()
    {
        string url = "https://taynljnktxrbkstipvla.supabase.co";
        string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InRheW5sam5rdHhyYmtzdGlwdmxhIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjM4NDQ2ODEsImV4cCI6MjAzOTQyMDY4MX0.bWvl8p0po-9i72L1au7JOS2OH9A3htO08oknjdLrdEk";
        var options = new SupabaseOptions { AutoConnectRealtime = true };

        _supabase = new Client(url, key, options);
        Debug.Log(_supabase.ToString());
        await _supabase.InitializeAsync();
    }
}