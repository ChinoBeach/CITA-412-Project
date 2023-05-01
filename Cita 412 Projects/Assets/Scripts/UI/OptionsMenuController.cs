using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider effectsVolumeSlider;
    [SerializeField] Slider dialogueVolumeSlider;
    
    [SerializeField] Toggle fullscreenToggle;
    
    [SerializeField] GameObject videoPanel;
    [SerializeField] GameObject audioPanel;
    [SerializeField] GameObject accessibilityPanel;
    [SerializeField] GameObject controlsPanel;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    
    void Start()
    {
        SetupSliderValues();    
    }
    
    void SetupSliderValues() {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolumeLevel");    
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolumeLevel");    
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("effectsVolumeLevel");    
        dialogueVolumeSlider.value = PlayerPrefs.GetFloat("dialogueVolumeLevel");    
    }
    
    void ApplyAudioChanges() {
        PlayerPrefs.SetFloat("masterVolumeLevel", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("musicVolumeLevel", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("effectsVolumeLevel", effectsVolumeSlider.value);
        PlayerPrefs.SetFloat("dialogueVolumeLevel", dialogueVolumeSlider.value);
    }
    
    public void ApplyChanges() {
        if (audioPanel.activeSelf) ApplyAudioChanges();
    }
    
    public void ClickVideoButton() {
        DisableAllPanels();
        videoPanel.SetActive(true);
    }
    
    public void ToggleFullscreen() {
        Screen.fullScreen = fullscreenToggle.isOn;
    }
    
    public void ClickAudioButton() {
        DisableAllPanels();
        audioPanel.SetActive(true);
    }
    
    public void ClickAccessibilityButton() {
        DisableAllPanels();
        accessibilityPanel.SetActive(true);
    }
    
    public void ClickControlsButton() {
        DisableAllPanels();
        controlsPanel.SetActive(true);
    }
    
    public void ClickBackButton() {
        DisableAllPanels();
        videoPanel.SetActive(true);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    
    void DisableAllPanels() {
        videoPanel.SetActive(false);
        audioPanel.SetActive(false);
        accessibilityPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }
}
