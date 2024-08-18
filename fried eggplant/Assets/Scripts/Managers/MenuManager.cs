using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneHandler.GotoScene("SampleScene", hasTransition: true);
    }
}
