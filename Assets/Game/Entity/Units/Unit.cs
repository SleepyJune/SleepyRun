using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Entity
{
    public Animator anim;

    public Dictionary<string, Buff> buffs = new Dictionary<string, Buff>();

    public int maxHealth = 10;
    public int defense = 0;
    
    [NonSerialized]
    public int health;

    [NonSerialized]
    public bool isRooted = false;

    [NonSerialized]
    public float flatMovespeedBonus = 0;
    [NonSerialized]
    public float highestSlowPercent = 0;

    [NonSerialized]
    public SlowDebuff highestSlowPercentBuff = null;

    [NonSerialized]
    public float baseMovespeed = 0;

    public bool isImmovable = false;

    private float lastBuffUpdateTime;
    
    public void CalculateSpeed()
    {
        speed = (baseMovespeed + flatMovespeedBonus) * (1 - highestSlowPercent);
    }
        
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

    public virtual void GainHealth(int gain)
    {
        if (!isDead)
        {
            health += gain;

            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
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
            if (buffs.ContainsKey(buff.buffName))
            {
                buffs[buff.buffName].endTime = Time.time + buff.duration; //refreshing the duration
            }
            else
            {
                buffs.Add(buff.buffName, buff);
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
        List<string> buffsToRemove = new List<string>();

        foreach(var pair in buffs)
        {
            var buff = pair.Value;

            if (buff.hasEnded)
            {
                buff.EndBuff();
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
