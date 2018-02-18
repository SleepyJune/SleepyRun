using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/ApplesToCollect")]
public class StageAppleToCollectChangeEvent : PreStageEvent
{
    public override string eventName { get { return "ApplesToCollect"; } }

    public int appleToCollect = 5;

    public override void ExecuteEvent()
    {
        GameManager.instance.stageEventManager.SetAppleToCollect(appleToCollect);

        isExecuted = true;
    }
}
