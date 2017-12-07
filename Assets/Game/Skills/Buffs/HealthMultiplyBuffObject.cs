using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Health Multiply Buff")]
public class HealthMultiplyBuffObject : BuffObject
{
    public float healthIncrease = 2;

    public override Buff Initialize(Unit source)
    {
        return new HealthMultiplyBuff(source, this, duration);
    }
}
