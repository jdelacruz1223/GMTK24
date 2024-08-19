using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GetInstance() { return instance; }
    public static GameManager instance;

    public string nextScene { get; private set; }
    public float currentTime { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        // Initialize Supabase
        if (SupabaseClient.GetInstance() != null) {
          await SupabaseClient.GetInstance().InitializeSupabase();
        }

        nextScene = "";
        currentTime = 0;
    }

    public void setNextScene(string name) => nextScene = name;
    public void setCurrentTime(float time) => currentTime = time;
    public void QuitGame() => Application.Quit();
    public void StartGame() => SceneHandler.GotoScene("Tutorial 1.1", hasTransition: true);
    public void GotoLeaderboard() => SceneHandler.GotoScene("Leaderboard", hasTransition: true);
    public void GoToNextLevel(string nextLevel) => SceneHandler.GotoScene(nextLevel, hasTransition: true);
    public void BackToMenu() => SceneHandler.GotoScene("MainMenuScene", hasTransition: true);
    public void RetryLevel() => SceneHandler.GotoScene(SceneManager.GetActiveScene().name, hasTransition: true);

    public void EndLevel()
    {
            
    }
}
