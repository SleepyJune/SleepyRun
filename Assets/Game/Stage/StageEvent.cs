using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class StageEvent : ScriptableObject
{
    [NonSerialized]
    public bool isExecuted = false;

    public abstract void ExecuteEvent();
}