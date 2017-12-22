using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Blind Debuff")]
public class BlindDebuffObject : BuffObject
{
    public override Buff Initialize(Unit source)
    {
        return new BlindDebuff(source, this, duration);
    }
}
