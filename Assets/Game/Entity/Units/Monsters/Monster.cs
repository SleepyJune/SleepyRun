using System;
using UnityEngine;

public class Monster : Unit
{    
    MonsterShatter shatterScript;
    
    [NonSerialized]
    public new Rigidbody rigidbody;
        
    [NonSerialized]
    public float timeSpawned;

    public BuffObject buffOnHit;

    void Awake()
    {        
        shatterScript = GetComponent<MonsterShatter>();
        rigidbody = GetComponent<Rigidbody>();
        
        id = GameManager.instance.GenerateEntityId();
        name = name.Replace("(Clone)", "");

        health = maxHealth;
        baseMovespeed = speed;
        baseDamage = damage;

        timeSpawned = Time.time;
    }
        
    void Update()
    {
        if (!isDead)
        {
            base.UnitUpdate();
                        
            if (speed != 0 && !isRooted && !isImmovable)
            {
                //var dir = new Vector3(0, 0, 1);
                transform.position -= Vector3.forward * speed * Time.deltaTime;

                if (anim)
                {
                    anim.SetFloat("speed", speed);
                }
            }
        }

        OnUnitUpdateEvent();
    }

    public override void TakeDamage(HitInfo hitInfo)
    {
        if (Time.time - timeSpawned < .25f)
        {
            //invincible for .25 seconds at spawn
            return;
        }

        if (canTakeDamage)
        {
            UnitTakeDamage(hitInfo);
                        
            if(hitInfo.damage > 0 && hitInfo.source == GameManager.instance.player)
            {
                GameManager.instance.comboManager.IncreaseComboCount();
                GameManager.instance.scoreManager.AddScoreOnHit(hitInfo);
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

            GameManager.instance.player.TakeDamage(
            new HitInfo
            {
                source = this,
                target = GameManager.instance.player,
                //hitStart = hitStart,
                //hitEnd = hitEnd,
                //force = force,
                damage = damage,
                //knockBackForce = knockBackForce,
                //hitParticle = particleOnHit,
                buffOnHit = buffOnHit,
            });

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
            OnDeathEvent(hitInfo);

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
