using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PointSpellUltimateUI : CombatUI
{
    public Spell spell;

    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public override void OnTouchStart(Touch touch)
    {
        var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);
        var newSpell = Instantiate(spell.gameObject, pos, Quaternion.identity);
    }

    public override void End()
    {

    }
}
