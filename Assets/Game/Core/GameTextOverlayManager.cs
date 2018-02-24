using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class GameTextOverlayManager : MonoBehaviour
{
    public Transform overlayParent;

    public GameObject victoryTextPrefab;
    public GameObject gameoverTextPrefab;
    public GameObject gameoverApplePrefab;
    public GameObject waveTextPrefab;
    
    public GameObject resumeTapTextPrefab;

    public GameObject skipLevelOverlayPrefab;

    public AdRewardWindow adRewardWindow; 
        
    GameObject resumeTapTextObject;
    
    void Start()
    {

    }

    public void ShowAdRewardButton(int currentLevel)
    {
        var rewardWindow = Instantiate(adRewardWindow, overlayParent);
        rewardWindow.SetReward(currentLevel);
    }

    public void CreateWaveText()
    {
        var stageEventManager = GameManager.instance.stageEventManager;
        var waveText = Instantiate(waveTextPrefab, overlayParent);

        waveText.SetActive(true);
        waveText.transform.Find("StageNumber").GetComponent<Text>().text = "Stage " + (stageEventManager.currentStageCount);

        if (stageEventManager.currentStageInfo.stageWaves.Length <= 1)
        {
            waveText.transform.Find("WaveNumber").GetComponent<Text>().text = "";
        }
        else
        {
            waveText.transform.Find("WaveNumber").GetComponent<Text>().text = "Wave " + (stageEventManager.currentWaveCount + 1);
        }

        waveText.GetComponent<Animation>().Play("StageNumberAnimation");
    }

    public void CreateResumeTapText()
    {
        if (resumeTapTextObject == null)
        {
            resumeTapTextObject = Instantiate(resumeTapTextPrefab, overlayParent);
            GameManager.instance.touchInputManager.touchStart += OnTouchResumeHandler;
        }
    }


    public void OnTouchResumeHandler(Touch touch)
    {
        GameManager.instance.ResumeGame();
        GameManager.instance.touchInputManager.touchStart -= OnTouchResumeHandler;
        Destroy(resumeTapTextObject);
    }

    public void CreateGameOverText()
    {
        var gameOverText = Instantiate(gameoverTextPrefab, overlayParent);
        //gameOverText.GetComponent<Animation>().Play("GameOverAnimation");
    }

    public void CreateGameOverOnAppleMissed()
    {
        var gameOverText = Instantiate(gameoverApplePrefab, overlayParent);
        //gameOverText.GetComponent<Animation>().Play("GameOverAnimation");
    }

    public void CreateVictoryText()
    {
        var victoryText = Instantiate(victoryTextPrefab, overlayParent);
        victoryText.GetComponent<Animation>().Play("GameOverAnimation");
    }

    public void CreateSkipLevelOverlay()
    {
        var skipLevelOverlay = Instantiate(skipLevelOverlayPrefab, overlayParent);
    }
}
