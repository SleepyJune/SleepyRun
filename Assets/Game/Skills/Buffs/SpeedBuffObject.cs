using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Speed Buff")]
public class SpeedBuffObject : BuffObject
{
    public float speedIncrease = 1;
    public GameObject effectTrailerPrefab;

    public override Buff Initialize(Unit source)
    {
        return new SpeedBuff(source, this, duration);
    }
}
