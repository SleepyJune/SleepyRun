using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class MonsterCondition : ScriptableObject
{
    [NonSerialized]
    public Monster monster;

    [NonSerialized]
    public bool isSatisfied;

    [NonSerialized]
    public MonsterConditionCollection conditionCollection;

    //public string description;

    public void BaseInitialize()
    {

    }

    public abstract void Initialize();
    public abstract void CheckCondition();
    public abstract void Destroy();
}
