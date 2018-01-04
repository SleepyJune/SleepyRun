using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class StageEvent : ScriptableObject
{
    public abstract string eventName { get;}

    [NonSerialized]
    public bool isExecuted = false;

    public Monster monster;

    public abstract void ExecuteEvent();
}