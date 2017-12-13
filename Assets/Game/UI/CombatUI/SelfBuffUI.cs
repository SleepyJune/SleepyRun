using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class SelfBuffUI : CombatUI
{
    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;

        var pos = GameManager.instance.player.transform.position;

        skill.CastSpell(pos, pos);

        //GameManager.instance.weaponManager.EndUltimate();
    }
}
