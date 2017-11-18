using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public enum SkillCastType
{
    SelfBuff,
    SelfAOE,
    Position,
    Unit
}

public abstract class Skill
{
    public SkillCastType castType;

    public float lastCastTime = 0;

    public float cooldown;

    public SkillInfo defaultSkillObject;

    public bool isActive = false;
        
    public Skill(SkillInfo skillObj)
    {
        defaultSkillObject = skillObj;
        castType = skillObj.castType;
        cooldown = skillObj.cooldown;

        defaultSkillObject.comabtUI.callBack = EndCast;
    }

    public bool canUseSkill
    {
        get
        {
            return
                !isActive
                && Time.time >= lastCastTime + cooldown                
                && GameManager.instance.player.canUseSkills;                
        }
    }

    public void UseSkill()
    {
        if (canUseSkill)
        {
            GameManager.instance.weaponManager.SwitchCombatUI(defaultSkillObject.comabtUI);
        }        
    }

    public abstract void Cast(Unit unit, Vector3 startPos, Vector3 endPos);

    public void EndCast()
    {
        GameManager.instance.weaponManager.SwitchCombatUI(null);
        isActive = false;
    }
}
