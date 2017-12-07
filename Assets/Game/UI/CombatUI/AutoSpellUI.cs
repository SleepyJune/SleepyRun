using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class AutoSpellUI : CombatUI
{
    public Spell spell;

    public int amount = 1;

    public Vector3 startPosition;
    public Vector3 direction;

    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;

        var pos = GameManager.instance.player.transform.position + startPosition;
        
        var newSpell = Instantiate(spell, pos, Quaternion.Euler(direction));
        newSpell.source = GameManager.instance.player;

        GameManager.instance.weaponManager.EndUltimate();
    }
}
