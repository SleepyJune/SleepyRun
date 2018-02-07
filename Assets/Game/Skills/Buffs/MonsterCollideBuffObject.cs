using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Monster Collide Buff")]
public class MonsterCollideBuffObject : BuffObject
{    
    public enum EffectFunctionType
    {
        None,
        Apple,
        GoldenApple,
    }

    public GameObject effectPrefab;

    public EffectFunctionType functionType = EffectFunctionType.None;

    public override Buff Initialize(Unit source)
    {
        return new MonsterCollideBuff(source, this, duration);
    }
}
