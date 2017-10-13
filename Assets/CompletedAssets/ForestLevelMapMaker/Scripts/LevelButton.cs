using UnityEngine;
using UnityEngine.UI;
public class LevelButton : MonoBehaviour {

    public GameObject LeftStar;
    public GameObject MiddleStar;
    public GameObject RightStar;
    public GameObject Lock;
    public Button button;
    public Text numberText;

    /// <summary>
    /// Set button interactable if button "active" or appropriate level is passed. Show stars or Lock image
    /// </summary>
    /// <param name="active"></param>
    /// <param name="activeStarsCount"></param>
    /// <param name="isPassed"></param>
    internal void SetActive(bool active, int activeStarsCount, bool isPassed)
    {
        LeftStar.SetActive(activeStarsCount > 1 && isPassed);
        MiddleStar.SetActive(activeStarsCount > 0 && isPassed);
        RightStar.SetActive(activeStarsCount > 2 && isPassed);
        button.interactable = active || isPassed;

        Lock.gameObject.SetActive(!isPassed && !active);
    }
}
