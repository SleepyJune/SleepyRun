using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class BuffObject : ScriptableObject
{
    public int buffID;
    public string buffName;
    public float duration;

    public bool continuousBuff = false;

    public abstract Buff Initialize(Unit source);
}
