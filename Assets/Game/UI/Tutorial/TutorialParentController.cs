using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class TutorialParentController : MonoBehaviour
{
    public GameObject tutorialObject;

    public Transform monsterTutorialParent;
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
}
