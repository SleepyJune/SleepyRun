using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Shield Buff")]
public class ShieldBuffObject : BuffObject
{
    public int shieldAmount = 100;

    public override Buff Initialize(Unit source)
    {
        return new ShieldBuff(source, this, duration);
    }
}
