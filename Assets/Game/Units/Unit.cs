using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Entity
{
    public Animator anim;

    public int maxHealth = 10;

    public Dictionary<int, Buff> buffs = new Dictionary<int, Buff>();
        
    [NonSerialized]
    public int health;

    public int defense = 0;

    public float GetRelativeSizeRatio()
    {
        var distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        return (15.5f * 55) / (distance * Camera.main.fieldOfView);
    }

    public void ApplyBuff(Buff buff)
    {
        if (buffs.ContainsKey(buff.buffID))
        {
            buffs[buff.buffID].endTime = Time.time + buff.duration;
        }
        else
        {
            buffs.Add(buff.buffID, buff);
            buff.ActivateBuff(this);
        }
    }

    public void CheckBuffs()
    {
        List<int> buffsToRemove = new List<int>();

        foreach(var pair in buffs)
        {
            var buff = pair.Value;

            if (buff.hasEnded)
            {
                buffsToRemove.Add(pair.Key);
            }
        }

        buffsToRemove.ForEach(key => buffs.Remove(key));
    }
}
