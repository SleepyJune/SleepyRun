using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Healing Buff")]
public class HealingBuffObject : BuffObject
{
    public float healing = 10;

    public override Buff Initialize(Unit source)
    {
        return new HealingBuff(source, this, duration);
    }
}
