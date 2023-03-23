using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour, IInteractable
{
    // Async Level Loader Object
    [SerializeField] private LevelLoader levelLoader;

    // Scene index of the desired island to teleport to
    //[SerializeField] private int islandSceneIndex;

    [SerializeField] private SceneName Scene;

    // True when teleporting is initiating. Used to stop multiple calls from happening
    private bool teleporting = false;

    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    
    public bool Interact(Interactinator9000 interactor) {
        Teleport();
        return true;
    }

    // Function to initiate the LevelLoader
    private void Teleport()
    {
        // Return if teleporting has already been initiated
        if (teleporting) return;

        // Set teleporting to true so no other calls can be made
        teleporting = true;

        // Load selected island with the levelloader
        levelLoader.LoadScene((int)Scene);
    }
}

// List of scenes by name for easy refrence.
public enum SceneName
{
    MainMenu = 0,
    MainIsland = 1,
    AirIsland = 2,
    EarthIsland = 3,
    FireIsland = 4,
    WaterIsland = 5
}