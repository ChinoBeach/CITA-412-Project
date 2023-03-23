using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Original Idea: https://youtu.be/THmW4YolDok
public class Interactinator9000 : MonoBehaviour
{
    [SerializeField] Transform interactionPoint;
    [SerializeField] float interactionPointRadius;
    [SerializeField] LayerMask interactableMask;
    [SerializeField] private Interactinator9000UI interactionUIPrompt;

    private readonly Collider[] colliders =  new Collider[3];
    [SerializeField] private int numFound;

    PlayerInputActions playerInput;
    InputAction interactButton;

    private IInteractable iInteractable;

    void Awake() {
        playerInput = new PlayerInputActions();
    }

    void OnEnable() {
        // Set interactButton input action to the interact button in player input actions
        interactButton = playerInput.Player.Interact;

        // Runs OnInteract when the interact button is pressed
        interactButton.started += ctx => OnInteract();
    }

    void Update() {
        // Updates numFound when the created sphere interacts with an object in its mask
        numFound = Physics.OverlapSphereNonAlloc(
            interactionPoint.position,
            interactionPointRadius,
            colliders,
            interactableMask
        );

        DisplayPrompt();
    }
    
    void DisplayPrompt() {
        // Return if there is nothing found hide the UI prompt
        if (numFound <= 0) {
            HidePrompt();
            return;
        }

        // Set interactable to the first element is colliders array
        iInteractable = colliders[0].GetComponent<IInteractable>();

        // Return if nothing was set
        if (iInteractable == null) return;

        // If the UI prompt is not displayed, display the prompt with the interacted objects text
        if (!interactionUIPrompt.isDisplayed) interactionUIPrompt.DisplayPrompt(iInteractable.InteractionPrompt);
    }

    // Method called when the "Interact" input actions is started
    void OnInteract() {
        // Return if there is nothing found or iInteractable is null
        if (numFound <= 0) return;
        if (iInteractable == null) return;

        // Interact with selected object
        iInteractable.Interact(this);
    }

    void HidePrompt() {
        // Set iInteractable to null
        iInteractable = null;
        // Hide UI interact prompt
        interactionUIPrompt.HidePrompt();
    }

    void OnDrawGizmos() {
        // Draws the interact sphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
