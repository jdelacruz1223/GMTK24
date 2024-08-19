using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GetInstance() { return instance; }
    public static GameManager instance;

    public string nextScene { get; private set; }
    public float currentTime { get; private set; }

    public float totalBooks { get; private set; }
    public float totalBookmarks { get; private set; }
    public List<LevelModel> levelUserStats { get; private set; }
    public UserStatsModel User { get; private set; }
    public string username { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    async void Start()
    {
        // Initialize Supabase
        if (SupabaseClient.GetInstance() != null)
        {
            await SupabaseClient.GetInstance().InitializeSupabase();
        }

        nextScene = "";
        currentTime = 0;
        levelUserStats = new List<LevelModel>();

        // Intialize User
        User.Name = "";
        User.levelStats = levelUserStats;
        User.totalTime = 0.0f;
        User.totalBookmarks = 0;
    }

    public void setNextScene(string name) => nextScene = name;
    public void setCurrentTime(float time) => currentTime = time;
    public void QuitGame() => Application.Quit();
    public void StartGame() => SceneHandler.GotoScene("Tutorial 1.1", hasTransition: true);
    public void GotoLeaderboard() => SceneHandler.GotoScene("Leaderboard", hasTransition: true);
    public void GoToNextLevel(string nextLevel) => SceneHandler.GotoScene(nextLevel, hasTransition: true);
    public void BackToMenu() => SceneHandler.GotoScene("MainMenuScene", hasTransition: true);
    public void RetryLevel() => SceneHandler.GotoScene(SceneManager.GetActiveScene().name, hasTransition: true);

    public void EndLevel(int level = 0)
    {
        TimeManager.instance.endLevel();
        levelUserStats.Append(new LevelModel { level = level, totalBookmarks = LevelManager.instance.totalBookmarks, elapsedTime = TimeManager.instance.getTime() });

        User.levelStats = levelUserStats;
        User.totalTime += TimeManager.instance.getTime();
        User.totalBookmarks += LevelManager.instance.totalBookmarks;

        LevelManager.instance.CompleteLevel();
    }

    public void SetUsername(string name) => username = name;
}
