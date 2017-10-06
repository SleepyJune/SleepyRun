using System;
using UnityEngine;

public class Monster : Unit
{
    public int damage = 1;

    MonsterShatter shatterScript;
    
    [NonSerialized]
    public new Rigidbody rigidbody;

    void Awake()
    {
        shatterScript = GetComponent<MonsterShatter>();
        rigidbody = GetComponent<Rigidbody>();

        health = maxHealth;
    }
    
    public void TakeDamage(HitInfo hitInfo)
    {
        health = Math.Max(0, health - hitInfo.damage);

        if(health == 0)
        {
            Death(hitInfo);
        }
    }

    public void RemoveFromStage()
    {
        if (!isDead)
        {
            GameManager.instance.comboManager.BreakCombo();

            GameManager.instance.player.TakeDamage(damage);

            var monsterDeathParticle = GetComponent<MonsterDeathParticle>();
            if (monsterDeathParticle)
            {
                monsterDeathParticle.CreateParticle();
            }
        }
    }

    public void Death(HitInfo hitInfo)
    {
        if (!isDead)
        {
            isDead = true;

            if (anim)
            {
                anim.SetTrigger("Die");
                anim.SetBool("isDead", true);
            }

            var monsterDeathParticle = GetComponent<MonsterDeathParticle>();
            if (monsterDeathParticle)
            {
                monsterDeathParticle.CreateParticle();
            }


            //DelayAction.Add(()=>shatterScript.MakeShattered(anim.transform),.5f);
                        
            shatterScript.MakeShattered(hitInfo);

            /*if (deathAnimation)
            {
                DeleteUnit(deathAnimation.length);
            }
            else
            {
                DeleteUnit(0);
            }*/
        }
    }
}
