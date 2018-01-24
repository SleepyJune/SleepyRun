using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class StatusBuff : Buff
{
    private StatusBuffObject buffObj;

    public StatusBuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as StatusBuffObject;
    }

    public override void ActivateBuff(Unit target)
    {
        this.unit = target;

        ToggleBuff(target, true);
    }

    public override void EndBuff()
    {
        if (unit)
        {
            ToggleBuff(unit, false);
        }
    }

    void ToggleBuff(Unit target, bool buffStatus)
    {
        var statusType = buffObj.statusBuffType;

        Debug.Log(statusType);

        if (statusType == StatusBuffObject.StatusBuffType.Root)
        {
            target.isRooted = buffStatus;
        }
        else if (statusType == StatusBuffObject.StatusBuffType.Blind)
        {
            target.isBlind = buffStatus;
        }
        else if (statusType == StatusBuffObject.StatusBuffType.Invincibility)
        {
            target.isInvincible = buffStatus;
        }
        else if (statusType == StatusBuffObject.StatusBuffType.Silence)
        {
            target.isSilenced = buffStatus;
        }
        else
        {

        }

        if(target is Player)
        {
            GameManager.instance.player.OnStatusChange(statusType, buffStatus);
        }
    }
}
