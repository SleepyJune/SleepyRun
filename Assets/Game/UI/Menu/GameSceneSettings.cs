using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class GameSceneSettings : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public CanvasGroup settingsWindowCanvasGroup;

    public void ToggleSettingMenu()
    {
        if(canvasGroup.alpha == 0)
        {
            GameManager.instance.PauseGame();

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            GameManager.instance.ResumeGame();

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
        SceneChanger.ChangeScene("IntroScreen2");
    }

    public void RestartLevel()
    {
        GameManager.instance.ResumeGame();
        SceneChanger.ChangeScene("GameScene");
    }
}
