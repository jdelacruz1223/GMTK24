using Assets.Scripts.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private async void Start()
    {
        await SupabaseClient.GetInstance().InitializeSupabase();
    }

    void Update()
    {
        
    }
}
