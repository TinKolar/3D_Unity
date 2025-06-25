using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas MainMenuCanvas;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void OnStartButton() 
    {
        gameManager.LoadScene(gameManager.GameOne,gameManager.MainMenu);
    }
    public void OnQuitButton()
    {
        gameManager.Quit();
    }



}
