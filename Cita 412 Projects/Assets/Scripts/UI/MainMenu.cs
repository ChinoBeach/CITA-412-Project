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
    [SerializeField] GameObject optionScreen;
    [SerializeField] LevelLoader levelLoader;
    #endregion

    #region Public Methods
    // Public Methods.

    public void NewGame()
    {
        levelLoader.LoadScene(1);
    }

    public void ContinueGame()
    {
        // TODO: Load save file in player prefs and load main scene
        NewGame();
    }
    public void OptionsButton()
    {
        // TODO: option moment
        menuScreen.SetActive(false);
        optionScreen.SetActive(true);
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