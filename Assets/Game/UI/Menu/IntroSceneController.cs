using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class IntroSceneController : MonoBehaviour
{
    public Text versionText;
        
    void Start()
    {
        if (versionText)
        {
            versionText.text = Application.version;
        }        
    }

    public void ToggleDebugMode()
    {
        var currentMode = PlayerPrefs.GetInt("DebuggingMode", 0) == 1;

        if (currentMode)
        {
            PlayerPrefs.SetInt("DebuggingMode", 0);
        }
        else
        {
            PlayerPrefs.SetInt("DebuggingMode", 1);
        }        
    }

    public void AddGold()
    {
        var currentMode = PlayerPrefs.GetInt("DebuggingMode", 0) == 1;

        if (currentMode)
        {
            MoneyManager.instance.IncreaseGold(500);
        }
    }

    public void StartGame()
    {
        SceneChanger.currentStageInfo = null;
        SceneChanger.ChangeScene("LoadingScene");
    }
}
