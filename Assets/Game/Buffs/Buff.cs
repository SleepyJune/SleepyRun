using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class Buff
{
    public float startTime;
    public float endTime;

    public int buffID;
    public string buffName;
    public float duration;

    [NonSerialized]
    public Unit unit;

    public Buff(BuffObject buffObj, float duration)
    {
        buffID = buffObj.buffID;
        buffName = buffObj.buffName;
        duration = buffObj.duration;

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
