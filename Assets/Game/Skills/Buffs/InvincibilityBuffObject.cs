using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Invincibility Buff")]
public class InvincibilityBuffObject : BuffObject
{
    public override Buff Initialize(Unit source)
    {
        return new InvincibilityBuff(source, this, duration);
    }
}
