using Assets.Scripts.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private void Start()
    {
        var supabaseClient = SupabaseClient.Instance.supabase;
        Debug.Log(supabaseClient.ToString());
    }

    void Update()
    {
        
    }
}
