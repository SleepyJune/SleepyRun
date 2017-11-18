using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class SpellSlotUI : MonoBehaviour
{
    public Image slotImage;
    public Skill skill;

    public GameObject iconObject;

    void Start()
    {

    }

    public void SetSkill(Skill skill)
    {
        this.skill = skill;

        slotImage.sprite = skill.defaultSkillObject.icon;
        iconObject.SetActive(true);
    }

    public void OnSpellSlotPressed()
    {
        skill.UseSkill();
    }
}
