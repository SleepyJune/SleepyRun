using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PointSpellUI : CombatUI
{
    public override void OnTouchStart(Touch touch)
    {
        var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

        skill.CastSpell(pos, pos);
    }
}
