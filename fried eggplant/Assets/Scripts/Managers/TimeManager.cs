using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    private TMP_Text text;
    private float startTime;
    private float currentTime;
    private float totalTime;
    private bool isPaused = false;
    private string currentScene;
    private GameObject textParent;
    [SerializeField] private float secondsPerBook = 1f;

    // Start is called before the first frame update
    void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        startTime = Time.time;
        totalTime = startTime;
        if (GameObject.Find("TimerText")) {
            textParent = GameObject.Find("TimerText");
            text = textParent.GetComponentInChildren<TMP_Text>();
            textParent.SetActive(false);
        }
        
        InvokeRepeating("Timer", 0.1f, Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScene != SceneManager.GetActiveScene().name) {
            resetTimer();
            unPauseTimer();
            currentScene = SceneManager.GetActiveScene().name;
            if (GameObject.Find("TimerText")) {
                textParent = GameObject.Find("TimerText");
                text = textParent.GetComponentInChildren<TMP_Text>();
                textParent.SetActive(false);
            }
        }
        totalTime = (!isPaused) ? totalTime + Time.deltaTime : totalTime;
        currentTime = totalTime - startTime;
        GameManager.GetInstance()?.setCurrentTime(currentTime);
    }
    public void Timer() {
        var t0 = (int) currentTime;
        var m = t0/60;
        var s = t0 - m*60;
        var ms = (int)((currentTime - t0)*100);
        if (text != null) {
          text.text = $"{m:00}:{s:00}:{ms:00}";
        }
    }
    [ContextMenu("Pause Timer")]
    public void pauseTimer() {
        CancelInvoke("Timer");
        isPaused = true;
    }
    public void endLevel() {
        currentTime -= secondsPerBook * DataManager.instance.booksAmount;

        var t0 = (int)currentTime;
        var m = t0 / 60;
        var s = t0 - m * 60;
        var ms = (int)((currentTime - t0) * 100);
        text.text = $"{m:00}:{s:00}:{ms:00}";
        if(textParent != null) textParent.SetActive(true);
        pauseTimer();
    }
    [ContextMenu("Unpause Timer")]
    public void unPauseTimer(){
        isPaused = false;
        InvokeRepeating("Timer", 0.1f, Time.deltaTime);
    }
    [ContextMenu("Reset Timer")]
    public void resetTimer(){
        startTime = Time.time;
        totalTime = startTime;
    }
    public float getTime() {
        return currentTime;
    }
}
