using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject loadingScreen;

    [Header("Sliders")]
    [SerializeField] private Slider loadingSlider;

    void Start()
    {
        StartCoroutine(LoadLevelAsync(GameManager.GetInstance().nextScene));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        yield return new WaitForSeconds(1);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;

            yield return null;
        }
    }
}
