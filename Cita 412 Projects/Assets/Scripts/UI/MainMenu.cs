using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    #region Variables
    // Variables.
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject exitScreen;
    #endregion

    #region Public Methods
    public void NewGame()
    {
        // TODO: Load the next scene
    }

    public void ContinueGame()
    {
        // TODO: Load save file in player prefs 
    }
    public void OptionsButton()
    {
        // TODO: option moment
    }

    public void CreditsButton()
    {
        creditsScreen.SetActive(true);
        menuScreen.SetActive(false);
    }

    public void CreditsBackButton()
    {
        creditsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    // Public Methods.
    public void ExitButton()
    {
        exitScreen.SetActive(true);
    }

    public void ExitCancelButton()
    {
        exitScreen.SetActive(false);
    }

    public void ExitConfirmButton()
    {
        Application.Quit();
    }
    #endregion
}