using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : CanvasGroupWindow
{
    public AudioListener audioListener;
    public AudioSource musicSource;
    public Slider volumeSlider;
    public Slider musicSlider;

    public Toggle[] qualityGroup;
    public SetResolution resolutionController;

    string volumeString = "volume";

    string musicString = "musicVolume";

    bool settingWindowOpen = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        float volume = .5f;
        float musicVolume = .5f;

        if (musicSource)
        {
            musicSource.ignoreListenerVolume = true;
        }

        if (PlayerPrefs.HasKey(volumeString))
        {
            volume = PlayerPrefs.GetFloat(volumeString);
            AudioListener.volume = volume;
            volumeSlider.value = volume;
        }

        if (PlayerPrefs.HasKey(musicString))
        {
            musicVolume = PlayerPrefs.GetFloat(musicString);

            if (musicSource)
            {
                musicSource.volume = musicVolume;            
                musicSlider.value = musicVolume;
            }
        }

        if (PlayerPrefs.HasKey("ScreenQuality"))
        {
            var toggle = qualityGroup[PlayerPrefs.GetInt("ScreenQuality")];

            if (toggle)
            {
                toggle.isOn = true;
            }
        }

        for(int i = 0; i < qualityGroup.Length; i++)
        {
            int qualityValue = i;

            qualityGroup[i].onValueChanged.AddListener((value) => resolutionController.SetPreferedQuality(value, qualityValue));
        }

        AdjustSoundEffectsVolume(volume);
        AdjustMusicVolume(musicVolume);
    }

    public void AdjustSoundEffectsVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(volumeString, volume);
    }

    public void AdjustMusicVolume(float volume)
    {
        if (musicSource)
        {
            musicSource.volume = volume;
        }

        PlayerPrefs.SetFloat(musicString, volume);
    }
}
