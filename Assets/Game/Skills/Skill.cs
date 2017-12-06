using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public enum SkillCastType
{
    SelfBuff,
    SelfAOE,
    Position,
    Unit
}

public abstract class Skill : MonoBehaviour
{
    public SkillCastType castType;

    public int id;
    public string spellName;
    public string description;
    public float castTime;
    public float cooldown;
    public float mana;
    public Sprite icon;

    public CombatUI comabtUI;
    
    [NonSerialized]
    public float lastCastTime = 0;

    [NonSerialized]
    public bool isActive = false;

    public abstract void Initialize();

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
            GameManager.instance.weaponManager.SwitchCombatUI(comabtUI);
        }
    }

    public abstract void Cast(Unit unit, Vector3 startPos, Vector3 endPos);

    public void EndCast()
    {
        GameManager.instance.weaponManager.SwitchCombatUI(null);
        isActive = false;
    }
}
