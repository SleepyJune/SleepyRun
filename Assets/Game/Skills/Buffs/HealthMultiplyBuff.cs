using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class HealthMultiplyBuff : Buff
{
    private HealthMultiplyBuffObject buffObj;        

    public HealthMultiplyBuff(BuffObject buffObj, float duration) : base(buffObj, duration)
    {
        this.buffObj = buffObj as HealthMultiplyBuffObject;
    }

    public override void ActivateBuff(Unit unit)
    {
        this.unit = unit;

        var healthPercent = unit.health / unit.maxHealth;

        unit.maxHealth = (int)Mathf.Round(unit.maxHealth * buffObj.healthIncrease);
        unit.health = (int)Mathf.Round(unit.maxHealth * healthPercent);

        unit.transform.localScale *= 2;
    }

    public override void EndBuff()
    {

    }
}
