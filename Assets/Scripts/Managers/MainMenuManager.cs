using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas MainMenuCanvas;
    [SerializeField] private Canvas LevelPickCanvas;

    [SerializeField] private GameObject backgroundImage;

    [SerializeField] private Image LockLvl1;
    [SerializeField] private Image LockLvl2;


    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        LevelPickCanvas.gameObject.SetActive(false);

        SetupBcgImg();
    }

    public void OnStartButton()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager is null in OnStartButton");
            return;
        }
        LevelPickCanvas.gameObject.SetActive(true);


        LockLvl1.gameObject.SetActive(!gameManager.levelTwoUnlocked);
        LockLvl2.gameObject.SetActive(!gameManager.levelThreeUnlocked);
    }

    public void OnLvlOneButton()
    {
        gameManager.LoadScene(gameManager.LevelOne, gameManager.MainMenu);

    }
    public void OnLvlTwoButton()
    {
        if (gameManager.levelTwoUnlocked)
            gameManager.LoadScene(gameManager.LevelTwo, gameManager.MainMenu);

    }
    public void OnLvlThreeButton()
    {
        if (gameManager.levelThreeUnlocked)
            gameManager.LoadScene(gameManager.LevelThree, gameManager.MainMenu);

    }


    public void OnCloseButton()
    {

        LevelPickCanvas.gameObject.SetActive(false);
    }


    public void OnTutorialButton()
    {
        gameManager.LoadScene(gameManager.Tutorial, gameManager.MainMenu);
    }
    public void OnQuitButton()
    {
        gameManager.Quit();
    }

    private void SetupBcgImg()
    {
        if (backgroundImage != null)
        {
            RectTransform rt = backgroundImage.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
            }
            else
            {
                Debug.LogWarning("backgroundImage does not have a RectTransform component.");
            }
        }
    }


}
