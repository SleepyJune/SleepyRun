using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Multi Buff")]
public class MultiBuffObject : BuffObject
{
    public BuffObject[] buffPrefabs;

    public GameObject effectTrailerPrefab;

    public override Buff Initialize(Unit source)
    {
        return new MultiBuff(source, this, duration);
    }
}
