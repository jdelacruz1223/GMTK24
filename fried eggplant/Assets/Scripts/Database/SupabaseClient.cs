using Supabase.Interfaces;
using Supabase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Buffers.Text;
using Assets.Scripts;

public class SupabaseClient : MonoBehaviour
{
    public static SupabaseClient GetInstance() { return me; }
    public static SupabaseClient me;

    private void Awake()
    {
        if (me != null)
        {
            Destroy(gameObject);
            return;
        }

        me = this;
    }

    public Supabase.Client Client {  get; private set; }

    public async Task InitializeSupabase()
    {
        string url = "tfIKRiFF8ps2QYAhD8tgPPmbhxMBkhSuJilZg5kFww8pO3jMjGiwJESEZ93WJndq";

        AESEncrypt aes = new AESEncrypt(Config.yek, Config.vi);

        Client = new Client(aes.Decrypt(url), aes.Decrypt(Config.key));
        await Client.InitializeAsync();
    }
}