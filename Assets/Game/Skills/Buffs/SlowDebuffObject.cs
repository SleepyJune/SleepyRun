using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Slow Debuff")]
public class SlowDebuffObject : BuffObject
{
    public float speedDecreasePercentage = .5f;
    public GameObject effectPrefab;

    public override Buff Initialize(Unit source)
    {
        return new SlowDebuff(source, this, duration);
    }
}
