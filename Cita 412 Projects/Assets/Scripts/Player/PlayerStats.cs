using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    private int playerHealth = 100;
    [SerializeField] private int maxPlayerHealth = 100;
    private int playerMana = 100;
    private int maxPlayerMana = 100;

    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    // Event when game ends
    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;

    // Player health property
    public int PlayerHealth {
        get {
            return playerHealth;
        }
        set {
            // Sets playerHealth value
            playerHealth = value;

            // Sets UI health elements
            healthSlider.value = playerHealth;
            healthText.text = $"{playerHealth}/{maxPlayerHealth}";

            // If the player health is less than or equal to one
            if (playerHealth <= 0) {
                // Set health text value to 0 so we do not display negative values
                healthText.text = $"0/{maxPlayerHealth}";

                GameOver();
            }
        }
    }

    void GameOver() {
        // Invoke gameover event
        onGameOver();
    }
}
