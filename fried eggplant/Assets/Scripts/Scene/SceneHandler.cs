using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// GotoScene
/// Goes to the scene directly, hasTransition will firstly load the Transition Scene and afterwards it will go the designated scene.
/// -> params (string name, hasTransition: bool) [hasTransition is optional]
/// e.g. SceneHandler.GotoScene("SampleScene") | SceneHandler.GotoScene("SampleScene", hasTransition: true)
/// 
/// AddScene
/// Additive Scening, will show another scene on top another scene.
/// -> params (string name)
/// e.g. AddScene("PlayerUI") // this is a scene!
/// 
/// UnloadScene
/// Unloads the specified scene
/// -> params (string name)
/// e.g. SceneHandler.UnloadScene("PlayerUI")
/// 
/// </summary>
public class SceneHandler
{
    /// <summary>
    /// Handles all of the scenes whether additive or not.
    /// Can add scenes, and unload scenes.
    /// </summary>

    public static void GotoScene(string i, bool hasTransition = false)
    {
        GameManager.GetInstance().setNextScene(i);

        if (hasTransition)
        {
            // From the transition onwards, they will be the one to handle the logic.
            SceneHelper.LoadScene("Transition");
            return;
        }

        SceneHelper.LoadScene(i);
    }
    public static void AddScene(string i) => SceneHelper.LoadScene(i, additive: true, setActive: true);
    public static void UnloadScene(string i) => SceneHelper.UnloadScene(i);
}
