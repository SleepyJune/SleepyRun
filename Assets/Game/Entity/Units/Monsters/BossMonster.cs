using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class BossMonster : Monster
{
    public Vector3 startPosition;

    [NonSerialized]
    public Player player;

    CanvasGroup hpBarCanvasGroup;
    Slider bossHPBar;

    Text bossHPText;
        
    void Start()
    {
        player = GameManager.instance.player;
        transform.position = player.transform.position + startPosition;

        //transform.SetParent(player.transform);

        var hpBarRect = GameManager.instance.canvas.Find("Hud/BossHPBar/HealthBar");
        hpBarCanvasGroup = hpBarRect.parent.GetComponent<CanvasGroup>();
        hpBarCanvasGroup.alpha = 1;

        bossHPText = hpBarRect.Find("Text").GetComponent<Text>();

        hpBarRect.parent.Find("Name").GetComponent<Text>().text = this.name;

        bossHPBar = hpBarRect.GetComponent<Slider>();
        bossHPBar.value = 100;

        GameManager.instance.SetBossFight(true);
    }

    public override void TakeDamage(HitInfo hitInfo)
    {        
        base.TakeDamage(hitInfo);

        if (!isDead)
        {
            var healthPercent = 100.0f * health / maxHealth;

            bossHPBar.value = healthPercent;
            bossHPText.text = Math.Round(healthPercent) + "%";
        }
    }


    public override void Death(HitInfo hitInfo)
    {
        if (!isDead)
        {
            isDead = true;

            GameManager.instance.monsterManager.AddKillCount(this);

            if (anim)
            {
                anim.SetTrigger("Die");
                anim.SetBool("isDead", true);
            }

            hpBarCanvasGroup.alpha = 0;

            Destroy(gameObject);
        }
    }
}
