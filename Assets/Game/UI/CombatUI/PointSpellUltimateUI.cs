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
        TouchInputManager.instance.touchStart += OnTouchStart;
    }

    private void OnTouchStart(Touch touch)
    {
        var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);
        var newSpell = Instantiate(spell.gameObject, pos, Quaternion.identity);
    }

    public override void End()
    {
        TouchInputManager.instance.touchStart -= OnTouchStart;
    }
}
