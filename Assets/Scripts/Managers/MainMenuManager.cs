using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas MainMenuCanvas;
    [SerializeField] private GameObject backgroundImage;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        SetupBcgImg();
    }

    public void OnStartButton() 
    {
        gameManager.LoadScene(gameManager.GameOne,gameManager.MainMenu);
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
