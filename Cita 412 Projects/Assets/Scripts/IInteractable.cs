using System.Collections;
using System.Collections.Generic;
// using UnityEngine;

// Original Idea: https://youtu.be/THmW4YolDok
public interface IInteractable {
    // Prompt for what the object is
    public string InteractionPrompt { get; }

    public bool Interact(Interactinator9000 interactor);
}
