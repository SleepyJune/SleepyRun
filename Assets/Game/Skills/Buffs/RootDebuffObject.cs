using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Root Debuff")]
public class RootDebuffObject : BuffObject
{
    public override Buff Initialize(Unit source)
    {
        return new RootDebuff(source, this, duration);
    }
}
