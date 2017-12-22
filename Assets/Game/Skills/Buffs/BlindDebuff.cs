using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class BlindDebuff : Buff
{
    private BlindDebuffObject buffObj;

    public BlindDebuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as BlindDebuffObject;
    }

    public override void ActivateBuff(Unit target)
    {
        this.unit = target;

        target.isBlind = true;

        if(target is Player)
        {
            GameManager.instance.player.ToggleBlindStatus();
        }
    }

    public override void EndBuff()
    {
        if (unit)
        {
            unit.isBlind = false;

            if (unit is Player)
            {
                GameManager.instance.player.ToggleBlindStatus();
            }
        }
    }
}
