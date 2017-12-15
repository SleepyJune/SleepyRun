using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class DamageBuff : Buff
{
    private DamageBuffObject buffObj;

    private GameObject effectTrailer;

    public DamageBuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as DamageBuffObject;
    }

    public override void ActivateBuff(Unit unit)
    {
        this.unit = unit;

        if (buffObj.isFlatDamageIncrease)
        {
            unit.bonusDamage += (int)buffObj.damageIncrease;
        }
        else
        {
            unit.damageMultiplier += buffObj.damageIncrease;
        }

        unit.CalculateTotalDamage();

        if (buffObj.effectTrailerPrefab)
        {
            effectTrailer = GameObject.Instantiate(buffObj.effectTrailerPrefab, unit.transform);
        }
    }

    public override void EndBuff()
    {
        if (buffObj.isFlatDamageIncrease)
        {
            unit.bonusDamage -= (int)buffObj.damageIncrease;
        }
        else
        {
            unit.damageMultiplier -= buffObj.damageIncrease;
        }

        unit.CalculateTotalDamage();

        if (effectTrailer)
        {
            GameObject.Destroy(effectTrailer);
        }
    }
}
