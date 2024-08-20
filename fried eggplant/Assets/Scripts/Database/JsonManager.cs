using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string id;
    public List<int> scenes;
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

    public static void WriteScene(int sceneId)
    {
        Player player = InitializeData();

        if (player != null)
        {
            if (player.scenes == null)
            {
                player.scenes = new List<int>();
            }

            player.scenes.Add(sceneId);

            string updatedData = JsonUtility.ToJson(player);

            File.WriteAllText(Config.dataPath, updatedData);
        }
    }

    public static bool IsSceneInList(int sceneId)
    {
        Player player = InitializeData();

        if (player != null && player.scenes != null)
        {
            return player.scenes.Contains(sceneId);
        }

        return false;
    }
}
