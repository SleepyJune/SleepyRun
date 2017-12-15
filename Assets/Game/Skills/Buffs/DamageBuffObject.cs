using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Damage Buff")]
public class DamageBuffObject : BuffObject
{
    public float damageIncrease = 1;

    public bool isFlatDamageIncrease = true;

    public GameObject effectTrailerPrefab;

    public override Buff Initialize(Unit source)
    {
        return new DamageBuff(source, this, duration);
    }
}
