using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class GameSceneSettings : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public CanvasGroupWindow settingsWindow;

    void Start()
    {
        
    }

    public void ToggleSettingMenu()
    {        
        if(canvasGroup.alpha == 0)
        {
            if (GameManager.instance.isGamePaused ||
            GameManager.instance.isGameOver ||
            GameManager.instance.isMovingToNextWave)
            {
                return;
            }

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
            if (!settingsWindow.isWindowOpen)
            {
                GameManager.instance.ResumeGameText();
            }

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    
    public void ToggleSettingsWindow()
    {
        if (!settingsWindow.isWindowOpen)
        {
            settingsWindow.ShowWindow();
        }
        else
        {
            GameManager.instance.ResumeGameText();

            settingsWindow.HideWindow();
        }
    }

    public void OpenSettingsWindow()
    {
        ToggleSettingsWindow();
        ToggleSettingMenu();
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

    public void ExitToLevelLoader()
    {
        GameManager.instance.ResumeGame();
        SceneChanger.ChangeScene("LevelLoader3");
    }
}
