using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    private int playerHealth = 100;
    [SerializeField] private int maxPlayerHealth = 100;
    [SerializeField] private float playerMana = 100f;
    [SerializeField] private float maxPlayerMana = 100f;
    [SerializeField] private float manaRegenRate = 2f;

    [SerializeField] private LevelLoader levelLoader;
    
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

    public float PlayerMana
    {
        get { return playerMana; }
        set 
        { 
            playerMana = value;

            if(playerMana >= maxPlayerMana)
            {
                playerMana = maxPlayerMana;
            }
        }
    }

    void GameOver() {
        // Invoke gameover event
        onGameOver();

        levelLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
