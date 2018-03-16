using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class IntroSceneController : MonoBehaviour
{
    public Text versionText;

    public StageInfoDatabase stageDatabase;

    void Start()
    {
        if (versionText)
        {
            versionText.text = Application.version;
        }        
    }

    public void ToggleDebugMode()
    {
        /*var currentMode = PlayerPrefs.GetInt("DebuggingMode", 0) == 1;

        if (currentMode)
        {
            PlayerPrefs.SetInt("DebuggingMode", 0);
        }
        else
        {
            PlayerPrefs.SetInt("DebuggingMode", 1);
        } */       
    }

    public void AddGold()
    {
        var currentMode = PlayerPrefs.GetInt("DebuggingMode", 0) == 1;

        if (currentMode)
        {
            //MoneyManager.instance.IncreaseGold(500);
        }
    }

    public void StartGame()
    {
        //SceneChanger.currentStageInfo = null;
        //SceneChanger.ChangeScene("LoadingScene");

        string topLevelString = "TopLevelPassed";
        var topPassedLevel = PlayerPrefs.GetInt(topLevelString, 0);
        
        if (topPassedLevel >= 10)
        {
            LoadLevel(9);
        }
        else if(topPassedLevel >= 2)
        {
            LoadLevel(4);
        }
        else
        {
            LoadLevel(0);
        }

    }

    public void LoadLevel(int stageID)
    {
        if (stageID >= stageDatabase.databaseArray.Length)
        {
            return;
        }

        StageInfo stageInfo = stageDatabase.databaseArray[stageID];

        if (stageInfo != null)
        {
            SceneChanger.currentStageInfo = stageInfo;
            SceneChanger.sceneToLoad = "GameScene";
            SceneChanger.ChangeScene("LoadingScene");
        }
    }
}
