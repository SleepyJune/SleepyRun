using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class AutoSpellUI : CombatUI
{
    public int amount = 1;

    public Vector3 startPosition;
    public Vector3 direction;

    public bool usePlayerX = false;

    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;

        Vector3 playerPos = GameManager.instance.player.transform.position;

        if (!usePlayerX)
        {
            playerPos.x = 0;
        }

        var pos = playerPos + startPosition;

        skill.CastSpell(pos, pos);

        GameManager.instance.weaponManager.EndUltimate();
    }
}
