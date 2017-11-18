using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public abstract class SkillInfo : MonoBehaviour
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

    public abstract Skill Initialize();
}
