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
        skill.spellslot = this;

        slotImage.sprite = skill.icon;
        iconObject.SetActive(true);
    }

    public void RemoveSkill()
    {
        if(skill != null)
        {
            Destroy(skill.gameObject);

            slotImage.sprite = null;
            iconObject.SetActive(false);
        }
    }

    public void OnSpellSlotPressed()
    {
        skill.UseSkill();
    }
}
