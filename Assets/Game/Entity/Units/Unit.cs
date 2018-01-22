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
    public int shield;

    [NonSerialized]
    public bool isRooted = false;

    [NonSerialized]
    public bool isBlind = false;

    [NonSerialized]
    public float flatMovespeedBonus = 0;
    [NonSerialized]
    public float highestSlowPercent = 0;

    [NonSerialized]
    public SlowDebuff highestSlowPercentBuff = null;

    [NonSerialized]
    public float baseMovespeed = 0;

    public bool isImmovable = false;

    public bool isInvincible = false;

    private float lastBuffUpdateTime;

    public int damage = 1;

    [NonSerialized]
    public int baseDamage = 1;
    [NonSerialized]
    public int bonusDamage = 0;
    [NonSerialized]
    public float damageMultiplier = 0;

    public delegate void TakeDamageHandler(HitInfo hitInfo, int finalDamage);
    public event TakeDamageHandler OnTakeDamage;

    public delegate void OnDeathHandler(HitInfo hitInfo);
    public event OnDeathHandler OnDeath;

    public delegate void Callback();
    public event Callback OnUnitUpdate;

    public abstract void TakeDamage(HitInfo hitInfo);

    protected virtual void OnTakeDamageEvent(HitInfo hitInfo, int finalDamage)
    {
        var evenHandler = OnTakeDamage;
        if(evenHandler != null)
        {
            evenHandler(hitInfo, finalDamage);
        }
    }

    protected virtual void OnUnitUpdateEvent()
    {
        var evenHandler = OnUnitUpdate;
        if (evenHandler != null)
        {
            evenHandler();
        }
    }

    protected virtual void OnDeathEvent(HitInfo hitInfo)
    {
        var evenHandler = OnDeath;
        if (evenHandler != null)
        {
            evenHandler(hitInfo);
        }
    }

    public bool canTakeDamage
    {
        get
        {
            return !isDead && !isInvincible && !GameManager.instance.isGameOver;
        }
    }
        
    public virtual int UnitTakeDamage(HitInfo hitInfo)
    {
        var finalDamage = CalculateDamage(hitInfo.damage);

        int damageAfterShield = finalDamage;
        if (finalDamage > 0 && shield > 0)
        {
            damageAfterShield = Math.Max(0, finalDamage - shield);

            shield = Math.Max(0, shield - finalDamage);
            shield = Math.Min(health, shield);
        }

        health = Mathf.Max(0, health - damageAfterShield);
                
        if (hitInfo.hitParticle)
        {
            Instantiate(hitInfo.hitParticle, transform.position, transform.rotation);
        }

        if (hitInfo.buffOnHit)
        {
            InitializeBuff(hitInfo.source, hitInfo.buffOnHit);
        }

        if (finalDamage > 0)
        {
            OnTakeDamageEvent(hitInfo, damageAfterShield);

            if(health > 0 && this is Monster)
            {
                GameManager.instance.damageTextManager.CreateDamageText(this, finalDamage.ToString(), DamageTextType.Physical);
            }
        }

        return finalDamage;
    }

    public void CalculateSpeed()
    {
        speed = (baseMovespeed + flatMovespeedBonus) * (1 - highestSlowPercent);
    }

    public void CalculateTotalDamage()
    {
        damage = (int)Math.Round((baseDamage + bonusDamage) * (1 + damageMultiplier));
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

    public virtual void GainShield(int shieldAmount)
    {
        if (!isDead)
        {
            if (shieldAmount > 0)
            {
                shield += shieldAmount;
                shield = Math.Min(health, shield);
            }
        }
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
            if (buffs.ContainsKey(buff.buffName)) //what if buff has ended
            {
                buffs[buff.buffName].endTime = Time.time + buff.duration; //refreshing the duration                
            }
            else
            {
                buffs.Add(buff.buffName, buff);
                buff.ActivateBuff(this);

                if(buff.duration == 0)
                {
                    buff.EndBuff();
                }
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
                        
            if (buff.hasEnded || (buff.endsOnOwnerDeath && buff.source == null))
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
