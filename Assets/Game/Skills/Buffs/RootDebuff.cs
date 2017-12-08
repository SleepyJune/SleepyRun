using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class RootDebuff : Buff
{
    private RootDebuffObject buffObj;

    public RootDebuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as RootDebuffObject;
    }

    public override void ActivateBuff(Unit target)
    {
        this.unit = target;

        target.isRooted = true;
    }

    public override void EndBuff()
    {
        if (unit)
        {
            unit.isRooted = false;
        }
    }
}
