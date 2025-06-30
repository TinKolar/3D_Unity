using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public string MainMenu = "MainMenuScene";
    public string LevelOne = "Game_1_Scene";
    public string LevelTwo = "Game_2_Scene";
    public string LevelThree = "Game_3_Scene";
    public string Tutorial = "TutorialScene";

    public bool levelTwoUnlocked=false;
    public bool levelThreeUnlocked=false;

    public string currentLevelScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadProgress();
        LoadScene("MainMenuScene");
    }
    public void LoadScene(string sceneName, string unloadSceneName = null)
    {
        StartCoroutine(LoadSceneRoutine(sceneName, unloadSceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, string unloadSceneName)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return loadOp;

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        if (loadedScene.IsValid())
        {
            SceneManager.SetActiveScene(loadedScene);
        }

        currentLevelScene = sceneName;

        if (!string.IsNullOrEmpty(unloadSceneName))
        {
            yield return SceneManager.UnloadSceneAsync(unloadSceneName);
        }
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelRoutine());
    }

    private IEnumerator RestartLevelRoutine()
    {
        string loaderScene = "LoaderScene";

        // Optional: ensure Loader is loaded
        if (!SceneManager.GetSceneByName(loaderScene).isLoaded)
            yield return SceneManager.LoadSceneAsync(loaderScene, LoadSceneMode.Additive);

        // Unload current level
        yield return SceneManager.UnloadSceneAsync(currentLevelScene);

        // Reload the level
        LoadScene(currentLevelScene);
        yield return null;

        // Optional: unload loader again if you want
        // yield return SceneManager.UnloadSceneAsync(loaderScene);
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("LevelTwoUnlocked", levelTwoUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("LevelThreeUnlocked", levelThreeUnlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        levelTwoUnlocked = PlayerPrefs.GetInt("LevelTwoUnlocked", 0) == 1;
        levelThreeUnlocked = PlayerPrefs.GetInt("LevelThreeUnlocked", 0) == 1;
    }

    public void Quit()
    {
        SaveProgress();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); //works only when we make a build for a game
#endif
    }
}
