using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class StageEventManager : MonoBehaviour
{
    public StageInfoDatabase stageInfoDatabase;

    StageInfo currentStageInfo;

    void Start()
    {
        currentStageInfo = SceneChanger.currentStageInfo;

        if (currentStageInfo == null)
        {
            string lastLevelString = "LastLevelPlayed";
            if (PlayerPrefs.HasKey(lastLevelString))
            {
                currentStageInfo = stageInfoDatabase.databaseArray[PlayerPrefs.GetInt(lastLevelString)-1];
            }
            else
            {
                currentStageInfo = stageInfoDatabase.databaseArray[0];
            }
        }

        ResetStage();
    }

    void ResetStage()
    {
        foreach (var stageEvent in currentStageInfo.stageEvents)
        {
            stageEvent.isExecuted = false;
        }
    }

    void Update()
    {
        if (currentStageInfo)
        {
            foreach (var stageEvent in currentStageInfo.stageEvents)
            {
                if (!stageEvent.isExecuted)
                {
                    stageEvent.ExecuteEvent();
                }
            }
        }
    }
}
