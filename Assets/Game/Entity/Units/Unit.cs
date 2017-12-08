using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Entity
{
    public Animator anim;

    public Dictionary<int, Buff> buffs = new Dictionary<int, Buff>();

    public int maxHealth = 10;
    public int defense = 0;
    
    [NonSerialized]
    public int health;


    private float lastBuffUpdateTime;

    public int CalculateDamage(float damage)
    {
        var finalDamage = damage;

        if(finalDamage == 0)
        {
            return 0;
        }

        finalDamage = Mathf.Max(1, finalDamage - defense);

        return (int)Mathf.Round(finalDamage);
    }

    public void SelfBuff(BuffObject buff)
    {
        InitializeBuff(this, buff);
    }

    public void InitializeBuff(Unit source, BuffObject buff)
    {
        var newBuff = buff.Initialize(source);

        ApplyBuff(newBuff);        
    }

    public void ApplyBuff(Buff buff)
    {
        if (!isDead)
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
    }

    public void UnitUpdate()
    {
        if(Time.time - lastBuffUpdateTime >= .25f)//check buff every 1/4 seconds
        {
            lastBuffUpdateTime = Time.time;
            CheckBuffs();
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
            else //buff hasn't ended
            {
                if (buff.continuousBuff)
                {
                    buff.ActivateBuff(this);
                }
            }
        }

        buffsToRemove.ForEach(key => buffs.Remove(key));        
    }
}
