using Assets.Scripts.Database;
using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public GameObject userCreationPanel;
    public TMP_InputField usernameTxt;

    [Header("Leaderboard Stats")]
    public TMP_Text totalTimePlayedTxt;
    public TMP_Text booksCollectedTxt;
    public TMP_Text totalScenesPlayedTxt;
    public TMP_Text usertxt;
    public GameObject placeholderMostTime;
    public GameObject mostBookCollected;

    void Start()
    {
        if (GameManager.GetInstance().hasId == false)
        {
            userCreationPanel.SetActive(true);
        }
        else userCreationPanel.SetActive(false);

        // Initialize Leaderboards here
        PopulateLeaderboards();
        UpdateStats();
    }

    async public void createUser()
    {
        if (usernameTxt.text.Length > 0)
        {
            bool status = await DBManager.instance.insertPlayer(usernameTxt.text);
            if (status)
            {
                userCreationPanel.SetActive(false);
            }
        }
    }

    void Update()
    {
        
    }

    async void UpdateStats()
    {
        if (userCreationPanel.activeInHierarchy == false)
        {
            var user = await DBManager.instance.FetchData(GameManager.GetInstance().user_id);
            var scenes = await DBManager.instance.GetScenes(GameManager.GetInstance().user_id);

            usertxt.text = user.Name;
            totalTimePlayedTxt.text = Util.floatToTime(user.totalTime);
            booksCollectedTxt.text = user.totalBookmarks.ToString();
            totalScenesPlayedTxt.text = scenes.ToString();
        }
    }

    async public void PopulateLeaderboards()
    {
        var elapsedResults = await DBManager.instance.FetchElapsedTimeLeaderboards();
        var bookCollected = await DBManager.instance.FetchMostBookCollectedLeaderboards();


        int index = 0;
        foreach (var leaderboard in elapsedResults)
        {
            var obj = Instantiate(placeholderMostTime, placeholderMostTime.transform.parent);
            obj.SetActive(true);

            TMP_Text[] texts = obj.gameObject.GetComponentsInChildren<TMP_Text>();

            TMP_Text pos = texts[0];
            TMP_Text name = texts[1];
            TMP_Text time = texts[2];

            pos.text = (index + 1).ToString();
            name.text = leaderboard.Name;
            time.text = Util.floatToTime(leaderboard.TotalTime);
            index++;
        }

        index = 0;
        foreach (var leaderboard in bookCollected)
        {
            var obj = Instantiate(mostBookCollected, mostBookCollected.transform.parent);
            obj.SetActive(true);

            TMP_Text[] texts = obj.gameObject.GetComponentsInChildren<TMP_Text>();

            TMP_Text pos = texts[0];
            TMP_Text name = texts[1];
            TMP_Text time = texts[2];

            pos.text = (index + 1).ToString();
            name.text = leaderboard.Name;
            time.text = leaderboard.Bookmark.ToString();
            index++;
        }
    }
}
