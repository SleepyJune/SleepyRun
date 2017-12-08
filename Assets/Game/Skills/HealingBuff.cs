using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class HealingBuff : Buff
{
    private HealingBuffObject buffObj;

    public HealingBuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as HealingBuffObject;
    }

    public override void ActivateBuff(Unit unit)
    {
        this.unit = unit;

        unit.GainHealth((int)Math.Round(buffObj.healing));
    }

    public override void EndBuff()
    {

    }
}
