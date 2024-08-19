using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Functions for Menu Buttons
    public void MainMenu()
    {
        GameManager.instance.BackToMenu();
    }
    public void RetryLevel()
    {
        GameManager.instance.RetryLevel();
    }
    public void NextLevel(string nextLevel)
    {
        GameManager.instance.GoToNextLevel(nextLevel);
    }
    public void Leaderboard()
    {
        GameManager.instance.GotoLeaderboard();
    }
    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
    public void StartGame()
    {
        GameManager.instance.StartGame();
    }
}
