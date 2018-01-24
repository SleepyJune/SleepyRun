using System;
using UnityEngine;

public enum MonsterCollisionMask
{
    None = 0,
    All = 0xFFFF,
    Good = 1,
    Neutral = 2,
    Bad = 4,
}

public class Monster : Unit
{    
    MonsterShatter shatterScript;
    
    [NonSerialized]
    public new Rigidbody rigidbody;
        
    [NonSerialized]
    public float timeSpawned;

    public MonsterCollisionMask monsterType = MonsterCollisionMask.Neutral;

    //public bool isBadMonster = true;

    public BuffObject buffOnHit;

    public Sprite image;

    Player player;

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

        player = GameManager.instance.player;
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

            RemoveOffStage();
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

        if ((hitInfo.monsterHitType & monsterType) == 0)
        {
            return;
        }

        if (canTakeDamage)
        {
            UnitTakeDamage(hitInfo);
                        
            if(hitInfo.damage > 0 && hitInfo.source == GameManager.instance.player)
            {
                //GameManager.instance.comboManager.IncreaseComboCount();
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

    void BadMonsterCollide()
    {
        GameManager.instance.comboManager.BreakCombo();

        GameManager.instance.player.TakeDamage(
                new HitInfo
                {
                    source = this,
                    target = GameManager.instance.player,
                    damage = damage,
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
    }

    void NeutralMonsterCollide()
    {
        if (buffOnHit != null)
        {
            GameManager.instance.player.TakeDamage(
                new HitInfo
                {
                    source = this,
                    target = GameManager.instance.player,
                    damage = 0,
                    buffOnHit = buffOnHit,
                });
        }

        var monsterDeathParticle = GetComponent<MonsterDeathParticle>();
        if (monsterDeathParticle)
        {
            monsterDeathParticle.CreateParticle();
        }

        if (anim)
        {

        }

        isDead = true;
    }

    void GoodMonsterCollide()
    {
        GameManager.instance.comboManager.IncreaseComboCount();
        GameManager.instance.monsterManager.AddMonsterCollectedCount(this);

        GameManager.instance.spawnPickupManager.TryPickup();

        GameManager.instance.monsterManager.CreateMoneyExplosion(transform.position);
    }

    public void CollideWithPlayer()
    {
        if (!isDead)
        {
            if (monsterType == MonsterCollisionMask.Bad)
            {
                BadMonsterCollide();
            }
            else if (monsterType == MonsterCollisionMask.Neutral)
            {
                NeutralMonsterCollide();
            }            
            else if (monsterType == MonsterCollisionMask.Good)
            {
                GoodMonsterCollide();
            }

            GameManager.instance.monsterManager.SetDead(this);
        }
    }

    public void RemoveOffStage()
    {
        if (player.transform.position.z - transform.position.z > 0)
        {
            if(monsterType == MonsterCollisionMask.Bad)
            {
                BadMonsterCollide();
            }
            else
            {
                GameManager.instance.monsterManager.AddMissedMonsterCount(this);
            }

            GameManager.instance.monsterManager.SetDead(this);
        }
    }

    public virtual void Death(HitInfo hitInfo)
    {
        if (!isDead)
        {
            OnDeathEvent(hitInfo);

            GameManager.instance.monsterManager.SetDead(this);
            GameManager.instance.monsterManager.AddKillCount(this);
            GameManager.instance.scoreManager.AddScoreOnMonsterKill(this);

            //GameManager.instance.spawnPickupManager.SpawnPickup(this);
                                          
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
