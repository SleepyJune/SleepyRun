using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class SpeedBuff : Buff
{
    private SpeedBuffObject buffObj;

    private GameObject effectTrailer;

    public SpeedBuff(BuffObject buffObj, float duration) : base(buffObj, duration)
    {
        this.buffObj = buffObj as SpeedBuffObject;
    }

    public override void ActivateBuff(Unit unit)
    {
        this.unit = unit;        
        unit.speed += buffObj.speedIncrease;

        if (buffObj.effectTrailerPrefab)
        {
            effectTrailer = GameObject.Instantiate(buffObj.effectTrailerPrefab, unit.transform);
        }
    }

    public override void EndBuff()
    {
        unit.speed -= buffObj.speedIncrease;

        if (effectTrailer)
        {
            GameObject.Destroy(effectTrailer);
        }
    }
}
