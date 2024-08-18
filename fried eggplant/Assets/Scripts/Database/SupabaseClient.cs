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
    /// <summary>
    /// Must exist in a certain point, much preferably in the beginning.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Initializes Supabase on Startup
    /// Using AES for encrypting the url and key for security purposes.
    /// </summary>
    /// <returns>None</returns>
    public async Task InitializeSupabase()
    {
        string url = "tfIKRiFF8ps2QYAhD8tgPPmbhxMBkhSuJilZg5kFww8pO3jMjGiwJESEZ93WJndq";

        AESEncrypt aes = new AESEncrypt(Config.yek, Config.vi);

        Client = new Client(aes.Decrypt(url), aes.Decrypt(Config.key));
        await Client.InitializeAsync();

        Debug.LogWarning("[Supabase] Connected to the Database as Client");
    }
}