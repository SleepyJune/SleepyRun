using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class GameSceneSettings : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public CanvasGroup settingsWindowCanvasGroup;

    public AudioListener audioListener;
    public AudioSource musicSource;
    public Slider volumeSlider;
    public Slider musicSlider;
    
    string volumeString = "volume";

    string musicString = "musicVolume";

    void Start()
    {
        float volume = .5f;
        float musicVolume = .5f;

        if (PlayerPrefs.HasKey(volumeString))
        {
            volume = PlayerPrefs.GetFloat(volumeString);
            AudioListener.volume = volume;
            volumeSlider.value = volume;
        }

        if (PlayerPrefs.HasKey(musicString))
        {
            musicVolume = PlayerPrefs.GetFloat(musicString);
            musicSource.volume = musicVolume;
            musicSlider.value = musicVolume;
        }

        AdjustVolume(volume);
        AdjustMusicVolume(musicVolume);
    }

    public void ToggleSettingMenu()
    {
        if(canvasGroup.alpha == 0)
        {
            if (!GameManager.instance.isGameOver)
            {
                GameManager.instance.PauseGame();
            }

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            if (!GameManager.instance.isGameOver)
            {
                GameManager.instance.ResumeGame();
            }

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void ToggleSettingsWindow()
    {
        if (settingsWindowCanvasGroup.alpha == 0)
        {
            GameManager.instance.PauseGame();

            settingsWindowCanvasGroup.alpha = 1;
            settingsWindowCanvasGroup.interactable = true;
            settingsWindowCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            GameManager.instance.ResumeGame();

            settingsWindowCanvasGroup.alpha = 0;
            settingsWindowCanvasGroup.interactable = false;
            settingsWindowCanvasGroup.blocksRaycasts = false;
        }
    }

    public void AdjustVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(volumeString, volume);
    }

    public void AdjustMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(musicString, volume);
    }

    public void OpenSettingsWindow()
    {
        ToggleSettingMenu();
        ToggleSettingsWindow();
    }

    public void CloseSettingsWindow()
    {
        ToggleSettingsWindow();
    }

    public void ExitLevel()
    {
        GameManager.instance.ResumeGame();
        SceneChanger.ChangeScene("IntroScreen3");
    }

    public void RestartLevel()
    {
        GameManager.instance.ResumeGame();
        SceneChanger.ChangeScene("GameScene");
    }
}
