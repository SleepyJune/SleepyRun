using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class StageEventManager : MonoBehaviour
{
    public StageInfoDatabase stageInfoDatabase;

    public StageInfo currentStageInfo;
    public StageWave currentStageWave;

    public int currentWaveCount = 0;

    void Start()
    {
        currentStageInfo = SceneChanger.currentStageInfo;

        if (currentStageInfo == null)
        {
            string lastLevelString = "LastLevelPlayed";
            if (PlayerPrefs.HasKey(lastLevelString))
            {
                currentStageInfo = stageInfoDatabase.databaseArray[PlayerPrefs.GetInt(lastLevelString) - 1];
            }
            else
            {
                currentStageInfo = stageInfoDatabase.databaseArray[0];
            }
        }

        if (currentStageInfo != null)
        {
            currentStageWave = currentStageInfo.stageWaves[0];
            ResetStage();
        }
    }

    void ResetStage()
    {
        foreach (var stageEvent in currentStageWave.stageEvents)
        {
            stageEvent.isExecuted = false;
        }
    }

    public bool AdvanceToNextWave()
    {
        if (currentWaveCount+1 < currentStageInfo.stageWaves.Length)
        {
            currentWaveCount++;
            currentStageWave = currentStageInfo.stageWaves[currentWaveCount];
            ResetStage();

            return false;
        }
        else
        {
            return true; //Game Over on Level Completion
        }        
    }

    void Update()
    {
        if (!GameManager.instance.isGamePaused && currentStageInfo && currentStageWave)
        {
            foreach (var stageEvent in currentStageWave.stageEvents)
            {
                if (!stageEvent.isExecuted)
                {
                    stageEvent.ExecuteEvent();
                }
            }
        }
    }
}
