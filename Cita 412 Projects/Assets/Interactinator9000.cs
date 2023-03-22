using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactinator9000 : MonoBehaviour
{
    [SerializeField] Transform interactionPoint;
    [SerializeField] float interactionPointRadius;
    [SerializeField] LayerMask interactableMask;

    private readonly Collider[] colliders =  new Collider[3];
    [SerializeField] private int numFound;

    PlayerInputActions playerInput;
    InputAction interactButton;

    void Awake() {
        playerInput = new PlayerInputActions();
    }

    void OnEnable() {
        interactButton = playerInput.Player.Interact;
        interactButton.started += ctx => OnInteract();
    }

    void Update() {
        numFound = Physics.OverlapSphereNonAlloc(
            interactionPoint.position,
            interactionPointRadius,
            colliders,
            interactableMask
        );
    }

    // Method called when the "Interact" input actions is started
    void OnInteract() {
        // Return if there is nothing found
        if (numFound <= 0) return;

        // Set interactable to the first element is colliders array
        var interactable = colliders[0].GetComponent<IInteractable>();

        // Return if nothing was set
        if (interactable == null) return;

        // Interact with selected object
        Debug.Log("Interacted");
        interactable.Interact(this);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
