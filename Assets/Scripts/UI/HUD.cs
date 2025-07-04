using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text keyEnemyText;

    [SerializeField] private Canvas looseCanvas; 
    [SerializeField] private Canvas optionsCanvas;
    [SerializeField] private Canvas winCanvas;

    GameManager gameManager;

    private float elapsedTime = 0f;

    private void Start()
    {
        gameManager = GameManager.Instance;

        looseCanvas.gameObject.SetActive(false);
        optionsCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timeText.text = $"Time: {minutes:00}:{seconds:00}";

        int numberOfKeyEnemies = GameObject.FindGameObjectsWithTag("KeyEnemy").Length;
        keyEnemyText.text = $"Key Enemies: {numberOfKeyEnemies}";

        OnEscPressed();
    }

    public void OnEscPressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f; 
            if (looseCanvas != null)
            {
               optionsCanvas.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Loose Canvas is not assigned.");
            }
        }
    }

    public void OnRestartButton()
    {
        Time.timeScale = 1f;

        looseCanvas.gameObject.SetActive(false);
        optionsCanvas.gameObject.SetActive(false);
        gameManager.RestartLevel();
    }

    public void OnQuitButton()
    {
        Time.timeScale = 1f;
        gameManager.LoadScene(gameManager.MainMenu, gameManager.currentLevelScene);
    }

    public void OnContiuneButton()
    {
        Time.timeScale = 1f;
        looseCanvas.gameObject.SetActive(false);
        optionsCanvas.gameObject.SetActive(false);

    }

    public void OnVictory()
    {
        Time.timeScale = 0f;
        winCanvas.gameObject.SetActive(true);
    }
    public void OnDefeat()
    {
        Time.timeScale = 0f;
        looseCanvas.gameObject.SetActive(true);
    }
}
