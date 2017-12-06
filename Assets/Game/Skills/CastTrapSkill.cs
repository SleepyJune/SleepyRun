using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class CastTrapSkill : Skill
{
    public Trap itemPrefab;

    public override void Initialize()
    {
        
    }

    public override void Cast(Unit unit, Vector3 startPos, Vector3 endPos)
    {
        GameObject.Instantiate(itemPrefab, endPos, Quaternion.identity);
        EndCast();
    }    
}
