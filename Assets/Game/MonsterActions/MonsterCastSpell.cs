using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "MonsterActions/Cast Spell")]
public class MonsterCastSpell : MonsterAction
{
    public Spell spell;

    public override bool Execute()
    {
        Instantiate(spell, monster.transform.position, monster.transform.rotation);
        spell.source = monster;

        return true;
    }
}
