using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Status Buff")]
public class StatusBuffObject : BuffObject
{
    public enum StatusBuffType
    {
        None,
        Root,
        Invincibility,
        Silence,
        Blind,
    }

    public StatusBuffType statusBuffType = StatusBuffType.None;

    public override Buff Initialize(Unit source)
    {
        return new StatusBuff(source, this, duration);
    }
}
