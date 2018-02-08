using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class StageEventManager : MonoBehaviour
{
    public StageInfoDatabase stageInfoDatabase;

    public StageInfo currentStageInfo;
    public StageWave currentStageWave;
            
    public int currentStageCount = 0;
    public int currentWaveCount = 0;

    public bool isSurvivalMode = false;
    
    public Transform killCountUI;

    public TutorialParentController tutorialController;


    Text killCountText;
    Image killCountImage;
    CanvasGroup killCountCanvasGroup;

    GameOverOnKillCountEvent victoryCondition;

    void Start()
    {
        GameManager.instance.isSurvivalMode = isSurvivalMode;
        
        currentStageInfo = SceneChanger.currentStageInfo;

        killCountCanvasGroup = killCountUI.GetComponent<CanvasGroup>();
        killCountText = killCountUI.Find("Text").GetComponent<Text>();
        killCountImage = killCountUI.Find("Image").GetComponent<Image>();

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
            currentStageCount = currentStageInfo.stageId;
            currentStageWave = currentStageInfo.stageWaves[0];
            ResetStage();

            GameManager.instance.MoveToNextArea();            
        }
    }

    public void StartTutorial()
    {
        var tutorialString = "Tutorial_Intro";

        if (!PlayerPrefs.HasKey(tutorialString))
        {
            tutorialController.StartTutorial();
            //PlayerPrefs.SetInt(tutorialString, 1);
        }        
    }

    void GetVictoryCondition()
    {
        killCountCanvasGroup.alpha = 0;

        foreach (var stageEvent in currentStageWave.stageEvents)
        {
            if(stageEvent is GameOverOnKillCountEvent)
            {
                victoryCondition = (GameOverOnKillCountEvent)stageEvent;

                killCountCanvasGroup.alpha = 1;
                killCountImage.sprite = victoryCondition.monster.image;
                killCountText.text = victoryCondition.killCount.ToString();
            }

            if(stageEvent is GameOverOnCountdown)
            {
                GameOverOnCountdown countdownEvent = (GameOverOnCountdown)stageEvent;

                GameManager.instance.timerManager.SetTime(countdownEvent.countdown);
            }
        }
    }

    void ResetStage()
    {
        foreach (var stageEvent in currentStageWave.stageEvents)
        {
            stageEvent.isExecuted = false;
        }

        GameManager.instance.monsterManager.ResetMonsterKillCount();
        GetVictoryCondition();
    }

    public void UpdateVictoryCondition(Monster monster, int killCount)
    {
        if (victoryCondition)
        {
            if(monster.name == victoryCondition.monster.name)
            {
                int amountLeft = victoryCondition.killCount - killCount;
                killCountText.text = amountLeft.ToString();
            }
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
            if (isSurvivalMode)
            {
                if(currentStageCount >= stageInfoDatabase.databaseArray.Length)
                {
                    return true; //completed the game
                }
                else
                {
                    currentStageInfo = stageInfoDatabase.databaseArray[currentStageCount];

                    if (currentStageInfo != null)
                    {
                        currentStageCount = currentStageInfo.stageId;
                        currentStageWave = currentStageInfo.stageWaves[0];
                        currentWaveCount = 0;
                        ResetStage();
                        
                        return false;
                    }
                    else
                    {
                        return true; //sth broke
                    }
                }
            }
            else
            {
                return true; //Game Over on Level Completion
            }
        }        
    }

    public void ShowMonsterInfo(Monster monster)
    {
        var monsterTutorialString = "Tutorial_MonsterInfo_" + monster.name;

        if (!PlayerPrefs.HasKey(monsterTutorialString))
        {
            if (!tutorialController.ShowMonsterInfo(monster))
            {
                DelayAction.Add(() => ShowMonsterInfo(monster), 5);
            }
            else
            {
                //PlayerPrefs.SetInt(monsterTutorialString, 1);
            }
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
