using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Async Level Loader Object
    [SerializeField] LevelLoader levelLoader;

    // Scene index of the desired island to teleport to
    [SerializeField] int islandSceneIndex;

    // True when teleporting is initiating. Used to stop multiple calls from happening
    bool teleporting = false;

    void OnTriggerEnter(Collider other) {
        // If the collided object has the player tag
        if (other.tag == "Player") {
            // Teleport
            Teleport();
        }
    }

    // Function to initiate the LevelLoader
    void Teleport() {
        // Return if teleporting has already been initiated
        if (teleporting) return;

        // Set teleporting to true so no other calls can be made
        teleporting = true;

        // Load selected island with the levelloader
        levelLoader.LoadScene(islandSceneIndex);
    }
}
