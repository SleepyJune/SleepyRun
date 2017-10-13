using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class StageEventManager : MonoBehaviour
{
    public StageInfo defaultStageInfo;

    StageInfo currentStageInfo;

    void Start()
    {
        currentStageInfo = SceneChanger.currentStageInfo;

        if (currentStageInfo == null)
        {
            currentStageInfo = defaultStageInfo;
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
