using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Original Idea: https://youtu.be/THmW4YolDok
public class Interactinator9000UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactPromptText;
    public bool isDisplayed = false;

    void Start() {
        interactPromptText.enabled = false;
    }

    public void DisplayPrompt(string prompt) {
        // Sets the prompt text to the interacted object
        interactPromptText.text = $"Press 'F' to interact with {prompt}";

        // Enables the prompt text
        interactPromptText.enabled = true;

        isDisplayed = true;
    }

    public void HidePrompt() {
        // Returns if the prompt is not displayed
        if (!isDisplayed) return;

        // Disables the prompt text
        interactPromptText.enabled = false;
        
        isDisplayed = false;
    }
}
