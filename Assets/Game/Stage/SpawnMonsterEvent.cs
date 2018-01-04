using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class SpawnMonsterEvent : StageEvent
{
    public override string eventName { get { return "Spawn Monster"; } }

    //public Monster monster;
    public float zPosition;

    public override void ExecuteEvent()
    {

    }
}
