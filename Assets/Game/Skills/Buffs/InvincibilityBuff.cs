using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class InvincibilityBuff : Buff
{
    private InvincibilityBuffObject buffObj;

    public InvincibilityBuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as InvincibilityBuffObject;
    }

    public override void ActivateBuff(Unit target)
    {
        this.unit = target;

        if(unit == GameManager.instance.player)
        {
            GameManager.instance.player.SetInvincibility(true);
        }
        else
        {
            target.isInvincible = true;
        }
    }

    public override void EndBuff()
    {            
        if (unit)
        {
            if (unit == GameManager.instance.player)
            {
                GameManager.instance.player.SetInvincibility(false);
            }
            else
            {
                unit.isInvincible = false;
            }
        }
    }
}
