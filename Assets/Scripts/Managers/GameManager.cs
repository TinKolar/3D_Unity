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
        LoadScene("MainMenuScene");
    }
    public void LoadScene(string sceneName, string unloadSceneName = null)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        if (unloadSceneName != null)
        {
            SceneManager.UnloadSceneAsync(unloadSceneName);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); //works only when we make a build for a game
#endif
    }
}
