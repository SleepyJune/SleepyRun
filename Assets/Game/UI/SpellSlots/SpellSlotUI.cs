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

    CanvasGroup canvasGroup;

    void Start()
    {
        anim = GetComponent<Animation>();

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        skill.spellslot = this;

        slotImage.sprite = skill.icon;
        iconObject.SetActive(true);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void RemoveSkill()
    {
        if(skill != null)
        {
            Destroy(skill.gameObject);

            slotImage.sprite = null;
            iconObject.SetActive(false);

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void DisableSkill(bool disable = true)
    {
        if (disable)
        {
            disabledObject.SetActive(true);
            isSkillDisabled = true;

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            disabledObject.SetActive(false);
            isSkillDisabled = false;

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
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
