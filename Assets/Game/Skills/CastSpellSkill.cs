using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class CastSpellSkill : Skill
{
    public Spell spellPrefab;

    protected override void Cast(Vector3 startPos, Vector3 endPos, Unit target = null)
    {
        var newSpell = GameObject.Instantiate(spellPrefab, endPos, Quaternion.identity);
        newSpell.source = owner;

        EndCast();
    }
}
