using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class ShieldBuff : Buff
{
    private ShieldBuffObject buffObj;

    private int shield = 0;

    public ShieldBuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as ShieldBuffObject;

        shield = this.buffObj.shieldAmount;
    }

    public override void ActivateBuff(Unit target)
    {
        this.unit = target;

        target.shield += shield;        
    }

    public int ShieldTakeDamage(int finalDamage)
    {
        if (shield == 0)
        {
            endTime = Time.time; //end the buff now

            return finalDamage;
        }
        else
        {
            var damageLeft = Math.Max(0, finalDamage - shield);

            shield = Math.Max(0, shield - finalDamage);

            return damageLeft;
        }
    }

    public override void EndBuff()
    {
        if (unit)
        {
            unit.shield = Math.Max(0,unit.shield - shield);
        }
    }
}
