using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCompanion : MonoBehaviour, IInteractable
{
    enum CompanionType {
        Air,
        Water,
        Fire,
        Earth
    }
    [SerializeField] GameObject[] companionModels;
    
    [SerializeField] CompanionType ungaBunga;
    
    private bool isSettingCompanion; 
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;

    public bool Interact(Interactinator9000 interactor) {
        SetCompanion();
        return true;
    }
    
    void SetCompanion() {
        if (isSettingCompanion) return;
        
        isSettingCompanion = true;
        
        switch (ungaBunga) {
            case CompanionType.Air:
                Debug.Log("Air island companion selected");
            break;
            case CompanionType.Water:
                Debug.Log("Water island companion selected");
            break;
            case CompanionType.Fire:
                Debug.Log("Fire companion selected");
            break;
            case CompanionType.Earth:
                Debug.Log("Earth companion selected");
            break;
            default:
                Debug.Log("Ruh Roh");
            break;
        }
        
        isSettingCompanion = false;
    }
}
