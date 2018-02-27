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

    public string description;

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

                if(highestSlowPercent > 0.05)
                {
                    var reverseBeltSpeed = GameManager.instance.floorManager.beltSpeed * highestSlowPercent;

                    transform.position += Vector3.forward * reverseBeltSpeed * Time.deltaTime;
                }

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

                    if (monsterType == MonsterCollisionMask.Good)
                    {
                        if (transform.position.x > 0.1)
                        {
                            transform.position -= new Vector3(.2f, 0, -3f);
                        }
                        else if (transform.position.x < -0.1)
                        {
                            transform.position += new Vector3(.2f, 0, 3f);
                        }
                        else
                        {
                            //unit.transform.position = new Vector3(buffObj.knockBackForce, 0, 0);
                        }
                    }
                    else
                    {
                        transform.position += new Vector3(0, 0, hitInfo.knockBackForce);
                    }

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

    void MonsterCollide()
    {
        if (monsterType == MonsterCollisionMask.Good)
        {
            GameManager.instance.comboManager.IncreaseComboCount();
            GameManager.instance.monsterManager.AddMonsterCollectedCount(this);
            if (buffOnHit)
            {
                GameManager.instance.player.InitializeBuff(this, buffOnHit);
            }
        }

        var damageTaken = damage;

        if (monsterType == MonsterCollisionMask.Good ||
            monsterType == MonsterCollisionMask.Neutral)
        {
            damageTaken = 0;
        }

        if (monsterType == MonsterCollisionMask.Bad ||
            monsterType == MonsterCollisionMask.Neutral)
        {
            GameManager.instance.player.TakeDamage(
                    new HitInfo
                    {
                        source = this,
                        target = GameManager.instance.player,
                        damage = damageTaken,
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

    void MonsterRemoveOffStage()
    {
        var damageTaken = monsterType == MonsterCollisionMask.Neutral ? 0 : 1;// damage;
        
        GameManager.instance.player.TakeDamage(
                new HitInfo
                {
                    source = this,
                    target = GameManager.instance.player,
                    damage = damageTaken,
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

        AddMissedApple();
    }

    public void CollideWithPlayer()
    {
        if (!isDead)
        {
            MonsterCollide();
            GameManager.instance.monsterManager.SetDead(this);
        }
    }

    public void RemoveOffStage()
    {
        if (player.transform.position.z - transform.position.z > 0)
        {
            MonsterRemoveOffStage();

            GameManager.instance.monsterManager.AddMissedMonsterCount(this);
            GameManager.instance.monsterManager.SetDead(this);
        }
    }

    public void AddMissedApple()
    {
        if (monsterType == MonsterCollisionMask.Good)
        {
            if (displayName == "GoldenApple")
            {
                GameManager.instance.scoreManager.AddMissedGoodApple(3);
            }
            else if (displayName == "Apple")
            {
                GameManager.instance.scoreManager.AddMissedGoodApple(1);
            }
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

            AddMissedApple();

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
