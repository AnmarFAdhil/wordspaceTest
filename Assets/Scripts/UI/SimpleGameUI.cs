using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private Health playerHealth;
    private WaveSpawner waveSpawner;
    private int score = 0;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerController>()?.GetComponent<Health>();
        waveSpawner = FindObjectOfType<WaveSpawner>();
        
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthDisplay;
            playerHealth.OnDeath += OnPlayerDeath;
        }
        
        if (waveSpawner != null)
        {
            waveSpawner.OnWaveChanged += OnWaveChanged;
        }
        
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        UpdateHealthDisplay(playerHealth != null ? playerHealth.GetCurrentHealth() : 0);
        UpdateScoreDisplay();
        UpdateWaveDisplay();
    }

    private void UpdateHealthDisplay(float health)
    {
        if (healthText != null)
            healthText.text = $"Health: {health:F0}";
    }

    private void UpdateWaveDisplay()
    {
        if (waveText != null && waveSpawner != null)
            waveText.text = $"Wave: {waveSpawner.GetCurrentWave()}";
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    private void OnWaveChanged(int waveNumber)
    {
        score += waveNumber * 100;
        UpdateWaveDisplay();
        UpdateScoreDisplay();
    }

    private void OnPlayerDeath()
    {
        if (healthText != null)
            healthText.text = "DEAD";
    }
}
