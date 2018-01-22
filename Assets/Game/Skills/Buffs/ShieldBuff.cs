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

        target.GainShield(this.buffObj.shieldAmount);
    }

    public override void EndBuff()
    {

    }
}
