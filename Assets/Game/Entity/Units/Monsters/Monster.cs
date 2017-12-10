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
    public event Callback OnDeath;

    [NonSerialized]
    public float timeSpawned;

    void Awake()
    {
        id = GameManager.instance.GenerateEntityId();
        
        shatterScript = GetComponent<MonsterShatter>();
        rigidbody = GetComponent<Rigidbody>();

        health = maxHealth;

        name = name.Replace("(Clone)", "");

        baseMovespeed = speed;

        timeSpawned = Time.time;
    }
        
    void Update()
    {
        if (!isDead)
        {
            base.UnitUpdate();
                        
            if (speed != 0 && !isRooted)
            {
                //var dir = new Vector3(0, 0, 1);
                transform.position -= Vector3.forward * speed * Time.deltaTime;

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
        if (Time.time - timeSpawned < .25f)
        {
            //invincible for .25 seconds at spawn
            return;
        }

        if (!isDead)
        {
            var finalDamage = CalculateDamage(hitInfo.damage);

            health = Mathf.Max(0, health - finalDamage);

            GameManager.instance.comboManager.IncreaseComboCount();
            GameManager.instance.scoreManager.AddScoreOnHit();

            if (hitInfo.hitParticle)
            {
                Instantiate(hitInfo.hitParticle, transform.position, transform.rotation);
            }

            if (hitInfo.buffOnHit)
            {
                InitializeBuff(hitInfo.source, hitInfo.buffOnHit);
            }

            if (finalDamage > 0 && OnTakeDamage != null)
            {
                OnTakeDamage();
            }
                        
            if (health == 0)
            {
                Death(hitInfo);
            }
            else//if monster not dead from the damage
            {
                if (hitInfo.knockBackForce != 0 && !isImmovable)
                {
                    var knockBackForce = new Vector3(0, 0, hitInfo.knockBackForce);
                    //rigidbody.AddForce(knockBackForce);

                    transform.position += knockBackForce;
                }

                if (finalDamage > 0)
                {
                    GameManager.instance.damageTextManager.CreateDamageText(this, finalDamage.ToString(), DamageTextType.Physical);
                }
            }
        }
    }

    public void CollideWithObstacle(Obstacle obstacle)
    {
        if (!isDead)
        {
            var start = obstacle.transform.position;
            var dir = (transform.position - start);
            dir.y = .15f;

            var force = dir * 50 * speed;

            var hitInfo =

                new HitInfo
                {
                    hitStart = start,
                    hitEnd = transform.position,
                    force = force,
                    damage = 1000,
                };

            Death(hitInfo);
        }
    }

    public void CollideWithPlayer()
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

            if (anim)
            {

            }

            isDead = true;
            Destroy(gameObject);
        }
    }

    public void RemoveFromStage()
    {
        if (!isDead)
        {
            /*GameManager.instance.comboManager.BreakCombo();

            GameManager.instance.player.TakeDamage(damage);

            var monsterDeathParticle = GetComponent<MonsterDeathParticle>();
            if (monsterDeathParticle)
            {
                monsterDeathParticle.CreateParticle();
            }*/
        }
    }

    public virtual void Death(HitInfo hitInfo)
    {
        if (!isDead)
        {
            if (OnDeath != null)
            {
                OnDeath();
            }

            isDead = true;

            GameManager.instance.monsterManager.AddKillCount(this);
            GameManager.instance.scoreManager.AddScoreOnMonsterKill(this);

            GameManager.instance.spawnPickupManager.SpawnPickup(this);

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

            if (shatterScript)
            {
                shatterScript.MakeShattered(hitInfo);
            }

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
