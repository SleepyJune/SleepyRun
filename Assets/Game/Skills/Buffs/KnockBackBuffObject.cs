using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/KnockBack Debuff")]
public class KnockBackBuffObject : BuffObject
{
    public float knockBackForce = 1;

    public override Buff Initialize(Unit source)
    {
        return new KnockBackBuff(source, this, duration);
    }
}
