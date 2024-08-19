using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public TMP_Text text;
    private float startTime;
    private float currentTime;
    private float totalTime;
    private bool isPaused = false;
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
    }
    void Start()
    {
        startTime = Time.time;
        totalTime = startTime;
        InvokeRepeating("Timer", 0.1f, Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
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
