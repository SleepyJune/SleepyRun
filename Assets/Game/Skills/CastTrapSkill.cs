using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class CastTrapSkill : Skill
{
    private CastTrapSkillInfo skillObject;

    public CastTrapSkill(SkillInfo skillObj) : base(skillObj)
    {
        skillObject = skillObject as CastTrapSkillInfo;
    }

    public override void Cast(Unit unit, Vector3 startPos, Vector3 endPos)
    {
        GameObject.Instantiate(skillObject.itemPrefab, endPos, Quaternion.identity);
        EndCast();
    }    
}
