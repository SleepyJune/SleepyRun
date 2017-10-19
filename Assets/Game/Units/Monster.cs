using System;
using UnityEngine;

public class Monster : Unit
{
    public int damage = 1;

    MonsterShatter shatterScript;
    
    [NonSerialized]
    public new Rigidbody rigidbody;

    public delegate void Callback();
    public event Callback OnTakeDamage;
    public event Callback OnMonsterUpdate;

    void Awake()
    {
        id = GameManager.instance.GenerateEntityId();
        
        shatterScript = GetComponent<MonsterShatter>();
        rigidbody = GetComponent<Rigidbody>();

        health = maxHealth;

        name = name.Replace("(Clone)", "");
    }

    void Update()
    {
        if (!isDead)
        {
            if (speed != 0)
            {
                var dir = new Vector3(0, 0, 1);
                transform.position -= dir * speed * Time.deltaTime;

                if (anim)
                {
                    anim.SetFloat("speed", speed);
                }
            }
        }

        if (OnMonsterUpdate != null)
        {
            OnMonsterUpdate();
        }
    }

    public virtual void TakeDamage(HitInfo hitInfo)
    {
        if (!isDead)
        {
            health = Math.Max(0, health - hitInfo.damage);

            GameManager.instance.comboManager.IncreaseComboCount();

            if (hitInfo.hitParticle)
            {
                Instantiate(hitInfo.hitParticle, transform.position, transform.rotation);
            }

            if (OnTakeDamage != null)
            {
                OnTakeDamage();
            }

            if (health == 0)
            {
                Death(hitInfo);
            }
            else
            {
                GameManager.instance.damageTextManager.CreateDamageText(this, hitInfo.damage);
            }
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

    public virtual void Death(HitInfo hitInfo)
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
