using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class CastTrapSkill : Skill
{
    public Trap itemPrefab;

    protected override void Cast(Vector3 startPos, Vector3 endPos, Unit target = null)
    {
        var newTrap = GameObject.Instantiate(itemPrefab, endPos, Quaternion.identity);
        newTrap.source = owner;

        EndCast();
    }    
}
