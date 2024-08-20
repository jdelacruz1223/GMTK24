using Assets.Scripts.Database;
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
    public string user_id { get; private set; }
    public bool hasId { get; set; }
    public bool dbError { get; set; }

    private void Awake()
    {
        ifError();
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    async void Start()
    {
        InitializeInitialData();
        
        // Initialize Supabase
        if (SupabaseClient.GetInstance() != null)
        {
            await SupabaseClient.GetInstance().InitializeSupabase();
        }

        // Initialize JsonManager, see if theres an existing user data
        var jsonUserId = JsonManager.InitializeData();
        if (jsonUserId != null)
        {
            hasId = true;
            SetUserID(jsonUserId.id);
            var data = await UserDBManager.instance.FetchData(jsonUserId.id);

            if (data != null)
            {
                Debug.Log(data.Name);
                Debug.LogWarning("[Supabase] fetched user. " + User.Name);
            } else
            {
                dbError = true;
                hasId = false;
            }
        } else
        {
            hasId = false;
        }
       
    }

    /// <summary>
    /// Make sure variables that are null must be set here.
    /// called first so that it wont conflict when retrieving data.
    /// </summary>
    void InitializeInitialData()
    {
        nextScene = "";
        currentTime = 0;
        levelUserStats = new List<LevelModel>();

        // Intialize User
        User = new UserStatsModel();
        User.Name = "";
        User.levelStats = levelUserStats;
        User.totalTime = 0.0f;
        User.totalBookmarks = 0;
        dbError = false;
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

    public void SetUserID(string id)
    {
        if (user_id == null && !hasId)
        {
            Debug.Log("Created Data Json");
            JsonManager.WriteID(id);
            user_id = id;
        } else
        {
            Debug.Log("User exists");
            user_id = id;
            hasId = true;
        }
    }

    void ifError()
    {
        #if UNITY_EDITOR
      Debug.Log("Unity Editor");
    #endif

    #if UNITY_IOS
      Debug.Log("iOS");
    #endif

    #if UNITY_STANDALONE_OSX
        Debug.Log("Standalone OSX");
    #endif

    #if UNITY_STANDALONE_WIN
      Debug.Log("Standalone Windows");
    #endif
    }
}
