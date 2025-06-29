using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text keyEnemyText;

    private float elapsedTime = 0f;

    private void Update()
    {
        // Update elapsed time
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timeText.text = $"Time: {minutes:00}:{seconds:00}";

        // Count KeyEnemy-tagged objects
        int numberOfKeyEnemies = GameObject.FindGameObjectsWithTag("KeyEnemy").Length;
        keyEnemyText.text = $"Key Enemies: {numberOfKeyEnemies}";
    }
}
