using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LevelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

    public GameObject LeftStar;
    public GameObject MiddleStar;
    public GameObject RightStar;
    public GameObject Lock;
    public Button button;
    public Text numberText;
    public int level;
    public StageInfo stageInfo;

    ScrollRect scrollRect;

    /// <summary>
    /// Set button interactable if button "active" or appropriate level is passed. Show stars or Lock image
    /// </summary>
    /// <param name="active"></param>
    /// <param name="activeStarsCount"></param>
    /// <param name="isPassed"></param>
    internal void SetActive(int level, bool active, int activeStarsCount, bool isPassed, StageInfo stageInfo)
    {
        this.level = level;
        this.stageInfo = stageInfo;

        name = "LevelButton " + (level+1);

        LeftStar.SetActive(activeStarsCount > 1 && isPassed);
        MiddleStar.SetActive(activeStarsCount > 0 && isPassed);
        RightStar.SetActive(activeStarsCount > 2 && isPassed);
        button.interactable = active || isPassed;        

        Lock.gameObject.SetActive(!isPassed && !active);

        scrollRect = transform.parent.parent.parent.GetComponent<ScrollRect>();

        transform.Find("Button").gameObject.AddComponent<LevelButtonHandler>();
    }

    public void LoadLevel()
    {
        if (stageInfo != null)
        {
            string lastLevelString = "LastLevelPlayed";
            PlayerPrefs.SetInt(lastLevelString, stageInfo.stageId);

            SceneChanger.currentStageInfo = stageInfo;
            SceneChanger.sceneToLoad = "GameScene";
            SceneChanger.ChangeScene("LoadingScene");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        scrollRect.enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        scrollRect.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scrollRect.enabled = true;
    }
}
