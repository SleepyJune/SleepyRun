using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Database/Skills Database")]
public class SkillDatabase : ScriptableObject
{
    public Skill[] allSkills = new Skill[0];
}
