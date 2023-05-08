using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }
    
    private AudioSource musicPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
        
        musicPlayer = GetComponent<AudioSource>();
        
        if (!musicPlayer.clip) Destroy(this.gameObject);

        UpdateMusicVolume();
        
        musicPlayer.loop = true;
        musicPlayer.Play();
    }

    public void UpdateMusicVolume() {
        float musicVolume = PlayerPrefs.GetFloat("musicVolumeLevel");
        musicPlayer.volume = (musicVolume) / 100;
    }
}
