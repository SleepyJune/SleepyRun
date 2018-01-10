using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public static LevelSelectManager instance = null;

    public StageInfoDatabase stageDatabase;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
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
            string lastLevelString = "LastLevelPlayed";
            PlayerPrefs.SetInt(lastLevelString, stageInfo.stageId);

            SceneChanger.currentStageInfo = stageInfo;
            SceneChanger.sceneToLoad = "GameScene";
            SceneChanger.ChangeScene("LoadingScene");
        }
    }
}
