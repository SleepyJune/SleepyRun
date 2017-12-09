using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class SlowDebuff : Buff
{
    public SlowDebuffObject buffObj;

    private GameObject effectTrailer;

    public SlowDebuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as SlowDebuffObject;
    }

    public override void ActivateBuff(Unit unit)
    {
        this.unit = unit;

        ApplySlowDebuff();

        if (buffObj.effectPrefab)
        {
            effectTrailer = GameObject.Instantiate(buffObj.effectPrefab, unit.transform);
        }
    }

    public override void EndBuff()
    {        
        if(unit.highestSlowPercentBuff == this)
        {
            unit.highestSlowPercent = 0;
            unit.highestSlowPercentBuff = null;
        }

        if (effectTrailer)
        {
            GameObject.Destroy(effectTrailer);
        }
    }

    public void ApplySlowDebuff()
    {
        var buffSlowPercent = buffObj.speedDecreasePercentage;

        if (unit.highestSlowPercentBuff != null)
        {
            if (unit.highestSlowPercent <= buffSlowPercent)
            {
                unit.highestSlowPercentBuff = this;
                unit.highestSlowPercent = buffSlowPercent;
            }
        }
        else
        {
            unit.highestSlowPercentBuff = this;
            unit.highestSlowPercent = buffSlowPercent;
        }

        unit.CalculateSpeed();
    }
}
