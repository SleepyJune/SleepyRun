using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/Tutorial event")]
public class TutorialEvent : StageEvent
{
    public override string eventName { get { return "Tutorial"; } }

    public enum TutorialType
    {
        Intro,
        Monster
    }

    public TutorialType tutorialType = TutorialType.Monster;

    public override void ExecuteEvent()
    {
        if (tutorialType == TutorialType.Monster)
        {
            GameManager.instance.stageEventManager.ShowMonsterInfo(monster);
            isExecuted = true;
        }
        else
        {
            GameManager.instance.stageEventManager.StartTutorial();
            isExecuted = true;
        }
    }    
}
