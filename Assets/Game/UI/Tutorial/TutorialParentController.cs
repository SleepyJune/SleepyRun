using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class TutorialParentController : MonoBehaviour
{
    public GameObject tutorialObject;

    public Transform monsterTutorialParent;

    public GameObject slideTutorial;
    public GameObject slashTutorial;

    public Transform overlayParent;

    TutorialController tutorialController;

    Animator anim;

    public bool tutorialLock = false;

    void Start()
    {
        anim = tutorialObject.GetComponent<Animator>();
        tutorialController = tutorialObject.GetComponent<TutorialController>();
    }

    public void SetSlashImage()
    {
        tutorialController.SetSlashImage();
    }

    public void StartTutorial()
    {        
        tutorialLock = true;

        GameManager.instance.PauseGame();

        tutorialObject.SetActive(true);
        anim.SetTrigger("Tutorial");
    }

    public bool ShowMonsterInfo(Monster monster)
    {
        if (!tutorialLock)
        {
            if (monster.tutorialObject != null)
            {
                tutorialLock = true;
                GameManager.instance.PauseGame();

                //tutorialObject.SetActive(true);

                //anim.SetTrigger("MonsterInfo");
                //tutorialController.ShowMonsterInfo(monster);

                var newTutorial = Instantiate(monster.tutorialObject, monsterTutorialParent);

                MonsterTutorialController monsterTutorial = newTutorial.GetComponent<MonsterTutorialController>();
                monsterTutorial.tutorialParent = this;
                //newTutorial.transform.SetParent(monsterTutorialParent, false);

                return true;
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    public void DisableTutorialOverlay()
    {
        tutorialObject.SetActive(false);
    }

    public void SetFiredAnimation()
    {
        tutorialObject.SetActive(true);
        anim.SetTrigger("Fired");
    }

    public void ShowSlideTutorial(bool isSlidingRight)
    {
        var tutorial = Instantiate(slideTutorial, overlayParent);

        tutorial.transform.Find("redbox").GetComponent<SlideTutorialController>().isSlidingRight = isSlidingRight;
    }

    public void ShowSlashTutorial(Monster monster)
    {
        var tutorial = Instantiate(slashTutorial, overlayParent);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(monster.transform.position);
        
        tutorial.transform.position = screenPos;

        var modifiedPos = tutorial.transform.localPosition;
        modifiedPos.y = 20f;

        tutorial.transform.localPosition = modifiedPos;
    }
}
