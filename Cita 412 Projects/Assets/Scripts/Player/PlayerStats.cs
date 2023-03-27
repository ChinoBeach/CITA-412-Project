using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int playerHealth = 100;
    private int playerMana = 100;

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

            // If the player health is less than or equal to one
            if (PlayerHealth <= 0) {
                GameOver();
            }
        }
    }

    void GameOver() {
        // Invoke gameover event
        onGameOver();
    }
}
