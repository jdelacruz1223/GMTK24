using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string id;
}

public class JsonManager
{
    public static Player InitializeData()
    {
        if (!Directory.Exists(Config.directoryPath))
            Directory.CreateDirectory(Config.directoryPath);

        if (File.Exists(Config.dataPath))
        {
            string str = File.ReadAllText(Config.dataPath);
            return JsonUtility.FromJson<Player>(str);
        }

        return null;
    }

    public static void WriteID(string id)
    {
        string str = JsonUtility.ToJson(new Player { id = id });
        File.WriteAllText(Config.dataPath, str);
    }
}
