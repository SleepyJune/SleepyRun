using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/Belt Speed Change")]
public class StageBeltSpeedChangeEvent : PreStageEvent
{
    public override string eventName { get { return "Belt Speed"; } }
    
    public float speed = 1.0f;

    public override void ExecuteEvent()
    {
        GameManager.instance.floorManager.beltSpeed = speed;

        isExecuted = true;
    }
}
