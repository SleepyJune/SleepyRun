using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterTutorialController : MonoBehaviour
{
    public TutorialParentController tutorialParent;

    public void DestroyAndResumeGame()
    {
        if (tutorialParent)
        {
            tutorialParent.tutorialLock = false;
            GameManager.instance.ResumeGame();
        }

        Destroy(gameObject);
    }
}
