using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public GameObject levelComplete;
    [SerializeField] GameObject[] Bookmarks;


    [Header("Bookmark Ranges (seconds)")]
    [Range(0.0f, 100f)] public float OneBookmark = 10f;
    [Range(0.0f, 100f)] public float TwoBookmark = 20f;
    [Range(0.0f, 100f)] public float ThreeBookmark = 30f;

    [Header("Sprites")]
    [SerializeField] Sprite[] imageBookmark;

    private float elapsedTime;

    public static LevelManager instance;

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
        elapsedTime = TimeManager.instance.getTime();
        CompleteLevel();

        int stars = CalculateStars(elapsedTime);
        DisplayStars(stars);
    }

    private int CalculateStars(float time)
    {
        if (time > OneBookmark && time < TwoBookmark)
        {
            return 1;
        }
        else if (time > TwoBookmark && time < ThreeBookmark)
        {
            return 2;
        }
        else if (time > ThreeBookmark)
        {
            return 3;
        }
        else
        {
            return 0;
        }
    }

    private void DisplayStars(int stars)
    {
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
}