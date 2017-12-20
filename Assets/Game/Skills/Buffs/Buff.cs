using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class Buff
{
    public float startTime;
    public float endTime;
    
    public string buffName;
    public float duration = 5;

    public Unit source;    
    public Unit unit;

    public bool continuousBuff;
    public bool endsOnOwnerDeath;

    public Buff(Unit source, BuffObject buffObj, float duration)
    {
        this.source = source;
        
        buffName = buffObj.buffName;
        duration = buffObj.duration;
        continuousBuff = buffObj.continuousBuff;
        endsOnOwnerDeath = buffObj.endsOnOwnerDeath;

        startTime = Time.time;
        endTime = Time.time + duration;
    }

    public bool hasEnded
    {
        get { return Time.time >= endTime; }
    }

    public abstract void ActivateBuff(Unit unit);
    public abstract void EndBuff();
}
