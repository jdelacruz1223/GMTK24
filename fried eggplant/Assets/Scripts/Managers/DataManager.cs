using Assets.Scripts.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private async void Start()
    {
        await SupabaseClient.GetInstance().InitializeSupabase();

        var result = await SupabaseClient.GetInstance().Client.From<Leaderboard>().Get();

        foreach (var player in result.Models)
        {
            Debug.LogWarning("Name: " + player.Name + " " + " Books Collected: " + player.Score.ToString());
        }
    }

    void Update()
    {
        
    }
}
