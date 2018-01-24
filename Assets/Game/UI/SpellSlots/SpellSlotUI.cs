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

    public GameObject disabledObject;

    public bool isSkillDisabled = false;

    Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();
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

    public void DisableSkill(bool disable = true)
    {
        if (disable)
        {
            disabledObject.SetActive(true);
            isSkillDisabled = true;
        }
        else
        {
            disabledObject.SetActive(false);
            isSkillDisabled = false;
        }
    }

    public void OnSpellSlotPressed()
    {
        if (!isSkillDisabled)
        {
            skill.UseSkill();
            anim.Play("ActivateSkillAnimation");
        }
    }
}
