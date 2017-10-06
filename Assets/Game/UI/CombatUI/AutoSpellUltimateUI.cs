using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class AutoSpellUltimateUI : CombatUI
{
    public Spell spell;

    public int amount = 1;

    public Vector3 startPosition;
    public Vector3 direction;

    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;

        var pos = GameManager.instance.player.transform.position + startPosition;
        
        var newSpell = Instantiate(spell.gameObject, pos, Quaternion.Euler(direction));
        
        GameManager.instance.weaponManager.EndUltimate();
    }

    public override void End()
    {

    }
}
