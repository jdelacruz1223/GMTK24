using Assets.Scripts.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GetInstance() { return instance; }
    public static GameManager instance;

    public string nextScene { get; private set; }
    public float currentTime { get; private set; }
    public float totalBooks { get; private set; }
    public UserStatsModel User { get; private set; }
    public string user_id { get; private set; }
    public bool hasId { get; set; }
    public bool dbError { get; set; }
    //public List<int> finishedScenes { get; set; }
    [SerializeField] private GameObject pauseMenu;

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
        // Initialize Supabase
        if (SupabaseClient.GetInstance() != null)
        {
            await SupabaseClient.GetInstance().InitializeSupabase();
        }

        InitializeInitialData();

        // Initialize JsonManager, see if theres an existing user data
        var jsonUserId = JsonManager.InitializeData();
        if (jsonUserId != null)
        {
            hasId = true;
            SetUserID(jsonUserId.id);
            //finishedScenes = jsonUserId.scenes;

            var data = await DBManager.instance.FetchData(jsonUserId.id);

            if (data != null)
            {
                User = data;
                User.totalTime = data.totalTime;
                User.totalBookmarks = data.totalBookmarks;
                User.Name = data.Name;
            }
            else
            {
                dbError = true;
                hasId = false;
            }
        }
        else
        {
            hasId = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
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

        // Intialize User
        User = new UserStatsModel();
        User.Name = "";
        User.totalTime = 0.0f;
        User.totalBookmarks = 0;
        dbError = false;
    }

    public void setNextScene(string name) => nextScene = name;
    public void setCurrentTime(float time) => currentTime = time;
    public void QuitGame() => Application.Quit();
    public void StartGame() => SceneHandler.GotoScene("1.1", hasTransition: true);
    public void GotoLeaderboard() => SceneHandler.GotoScene("Leaderboard", hasTransition: true);
    public void GoToNextLevel(string nextLevel) => SceneHandler.GotoScene(nextLevel, hasTransition: true);
    public void BackToMenu() => SceneHandler.GotoScene("MainMenuScene", hasTransition: true);
    public void RetryLevel() => SceneHandler.GotoScene(SceneManager.GetActiveScene().name, hasTransition: true);

    async public void EndLevel(int level = 0)
    {
        Debug.LogWarning("End Level");
        TimeManager.instance.endLevel();
        User.totalTime += TimeManager.instance.getTime();
        User.totalBookmarks += LevelManager.instance.totalBookmarks;

        Debug.LogWarning("Adding user to Leaderboards & Updating User Stats");
        await DBManager.instance.AddUserFromLeaderboard(user_id, SceneManager.GetActiveScene().buildIndex, User.Name, User.totalTime, User.totalBookmarks);
        await DBManager.instance.UpdatePlayer(user_id, User);

        Debug.LogWarning("Level Manager Complete Level Initalized");
        LevelManager.instance.CompleteLevel();
    }

    public void SetUserID(string id)
    {
        if (user_id == null && !hasId)
        {
            Debug.Log("Created Data Json");
            JsonManager.WriteID(id);
            user_id = id;
        }
        else
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
