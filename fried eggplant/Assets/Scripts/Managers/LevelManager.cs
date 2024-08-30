using Assets.Scripts.Database;
using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public GameObject timerText;
    [SerializeField] GameObject[] Bookmarks;


    [Header("Bookmark Ranges (under x seconds)")]
    [Range(0.0f, 100f)] public float OneBookmark = 10f;
    [Range(0.0f, 100f)] public float TwoBookmark = 20f;
    [Range(0.0f, 100f)] public float ThreeBookmark = 30f;

    [Header("Sprites")]
    [SerializeField] Sprite[] imageBookmark;

    [Header("Leaderboards")]
    public GameObject LbPlaceholder;

    private float elapsedTime;

    public static LevelManager instance;

    public int totalBookmarks { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void CompleteLevel()
    {
        Debug.LogWarning("Populate Leaderboards");
        PopulateLeaderboards();
        timerText.SetActive(true);
        elapsedTime = TimeManager.instance.getTime();

        Debug.LogWarning("Populate Display Stars");
        int stars = CalculateStars(elapsedTime);
        DisplayStars(stars);
    }

    private int CalculateStars(float time)
    {
        Debug.Log(time);
        if (time <= ThreeBookmark)
        {
            Debug.Log(3);
            return 3;
        }
        else if (time <= TwoBookmark)
        {
            Debug.Log(2);
            return 2;
        }
        else if (time <= OneBookmark)
        {
            Debug.Log(1);
            return 1;
        }
        else
        {
            return 0;
        }

    }

    private void DisplayStars(int stars)
    {
        totalBookmarks += stars;

        for (int i = 0; i < stars; i++)
        {
            GameObject bookmark = Bookmarks[i];
            UnityEngine.UI.Image image = bookmark.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.sprite = imageBookmark[i];
            }
        }
    }

    async public void PopulateLeaderboards()
    {
        Debug.LogWarning(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        var results = await DBManager.instance.FetchLeaderboards(0, 10);

        int index = 0;
        foreach (var leaderboard in results)
        {
            var obj = Instantiate(LbPlaceholder, LbPlaceholder.transform.parent);
            obj.SetActive(true);

            TMP_Text[] texts = obj.gameObject.GetComponentsInChildren<TMP_Text>();

            TMP_Text pos = texts[0];
            TMP_Text name = texts[1];
            TMP_Text time = texts[2];

            pos.text = (index + 1).ToString();
            name.text = leaderboard.playerName;
            time.text = Util.floatToTime(leaderboard.elapsedTime);
            index++;
        }
    }
}