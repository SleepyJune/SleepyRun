using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MultiBuff : Buff
{
    private MultiBuffObject buffObj;

    private List<Buff> buffs = new List<Buff>();

    public GameObject effectTrailer;

    public MultiBuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as MultiBuffObject;

        foreach (var buff in this.buffObj.buffPrefabs)
        {
            buffs.Add(buff.Initialize(source));
        }
    }

    public override void ActivateBuff(Unit unit)
    {
        this.unit = unit;

        buffs.ForEach(buff => buff.ActivateBuff(unit));

        if (buffObj.effectTrailerPrefab)
        {
            effectTrailer = GameObject.Instantiate(buffObj.effectTrailerPrefab, unit.transform);
        }
    }

    public override void EndBuff()
    {
        if (effectTrailer)
        {
            GameObject.Destroy(effectTrailer);
        }
    }
}
