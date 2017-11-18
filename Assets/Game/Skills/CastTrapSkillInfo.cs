using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

//[CreateAssetMenu(menuName = "Skills/Cast Traps")]
public class CastTrapSkillInfo : SkillInfo
{
    public Traps itemPrefab;

    public override Skill Initialize()
    {
        return new CastTrapSkill(this);
    }
}
