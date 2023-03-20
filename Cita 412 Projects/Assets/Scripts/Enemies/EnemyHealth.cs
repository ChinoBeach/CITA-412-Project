using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health;

    public void DealDamage(float damage) {
        // Subract damage taken from health
        health -= damage;

        // If health is less than or equal to zero destroy the enemy
        if (health <= 0) DestroyEnemy();
    }

    void DestroyEnemy() {
        // Destroy Enemy game object
        Destroy(this.gameObject);
    }
}
