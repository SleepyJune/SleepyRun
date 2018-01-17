using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LevelButton : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public GameObject LeftStar;
    public GameObject MiddleStar;
    public GameObject RightStar;
    public GameObject Lock;
    public Button button;
    public Text numberText;
    public int level;

    GameObject levelSelectManager;
    
    ScrollRect scrollRect;

    /// <summary>
    /// Set button interactable if button "active" or appropriate level is passed. Show stars or Lock image
    /// </summary>
    /// <param name="active"></param>
    /// <param name="activeStarsCount"></param>
    /// <param name="isPassed"></param>
    public void SetActive(int level, bool active, int activeStarsCount, bool isPassed, GameObject levelSelectManager)
    {
        this.level = level;
        this.levelSelectManager = levelSelectManager;

        name = "LevelButton " + (level+1);

        LeftStar.SetActive(activeStarsCount > 1 && isPassed);
        MiddleStar.SetActive(activeStarsCount > 0 && isPassed);
        RightStar.SetActive(activeStarsCount > 2 && isPassed);
        button.interactable = active || isPassed;        

        Lock.gameObject.SetActive(!isPassed && !active);

        scrollRect = transform.parent.parent.parent.GetComponent<ScrollRect>();

        numberText.text = level.ToString();

        //transform.Find("Button").gameObject.AddComponent<LevelButtonHandler>();
    }

    public void LoadLevel()
    {
        levelSelectManager.SendMessage("LoadLevel", level-1);
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
