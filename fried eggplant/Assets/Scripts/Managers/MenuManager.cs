using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Functions for Menu Buttons
    public void MainMenu()
    {
        Time.timeScale = 1;
        GameManager.instance.BackToMenu();
    }
    public void RetryLevel()
    {
        Time.timeScale = 1;
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
