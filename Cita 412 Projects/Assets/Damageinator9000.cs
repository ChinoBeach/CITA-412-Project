using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageinator9000 : MonoBehaviour
{
    // Amount of damage the weapon does
    public int damage = 40;

    // Whether or not the weapon has done damage during the animation
    public bool hasDoneDamage = false;

    void OnTriggerEnter(Collider collider) {
        // If the collider hits the player and damage has not been done yet
        if (collider.CompareTag("Player") && !hasDoneDamage) {
            // Deal amount of damage to player health
            collider.gameObject.GetComponent<PlayerStats>().PlayerHealth -= damage;

            hasDoneDamage = true;
        }
    }

    // Reset hasDoneDamage on last frame of animation
    public void ResetHasDoneDamage() {
        hasDoneDamage = false;
    }
}
