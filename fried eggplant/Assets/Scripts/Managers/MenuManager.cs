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
        SceneHandler.GotoScene("Tutorial Scene", hasTransition: true);
    }
    
    public void GotoLeaderboard() => SceneHandler.GotoScene("Leaderboard");
    
    public void BackToMenu()
    {

    }
    
    public void Retry()
    {
        
    }
}
