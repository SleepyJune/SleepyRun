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
    public Unit owner;
    
    [NonSerialized]
    public float lastCastTime = 0;

    [NonSerialized]
    public bool isActive = false;

    [NonSerialized]
    public SpellSlotUI spellslot;

    public AudioClip audioClip;

    public virtual void Initialize(Unit owner)
    {
        this.owner = owner;
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
            GameManager.instance.weaponManager.SwitchCombatUI(comabtUI);
        }
    }

    protected abstract void Cast(Vector3 startPos, Vector3 endPos, Unit target = null);

    public void CastSpell(Vector3 startPos, Vector3 endPos, Unit target = null)
    {
        GameManager.instance.scoreManager.AddScore(50);

        Cast(startPos, endPos, target);

        if (audioClip)
        {
            GameManager.instance.audioSource.PlayOneShot(audioClip);
        }
    }

    public void EndCast()
    {
        GameManager.instance.weaponManager.SwitchCombatUI(null);
        isActive = false;

        spellslot.RemoveSkill();
    }
}
