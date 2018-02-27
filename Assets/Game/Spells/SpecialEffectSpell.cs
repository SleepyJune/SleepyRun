using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class SpecialEffectSpell : Spell
{
    public float effectTime = 0;

    public enum EffectType
    {
        None,
        SlowBeltSpeed,
    }

    public EffectType effectType = EffectType.None;

    public float effectValue = 0.0f;

    float previousValue = 0.0f;

    void Start()
    {
        ApplyEffect();

        DelayAction.Add(EndEffect, effectTime);
    }

    void ApplyEffect()
    {
        if(effectType == EffectType.SlowBeltSpeed)
        {
            GameManager.instance.floorManager.SetPercentSlow(effectValue, effectTime);
        }
    }

    void EndEffect()
    {
        Death();
    }

    public override void Death()
    {
        isDead = true;
        Destroy(transform.gameObject);
    }
}
