using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Player : Unit
{
    [System.NonSerialized]
    public bool isBossFight = false;

    public Lane destinationLane = Lane.mid;
    [System.NonSerialized]
    public Lane currentLane = Lane.mid;
    
    public UIUnitFrame_Bar healthBarScript;

    public WeaponButton playerPortrait;
    public Transform playerPortraitTransform;

    [System.NonSerialized]
    public Skill[] skills = new Skill[3];

    public SpellSlotUI spellSlotPrefab;
    public Transform spellSlotTransform;

    SpellSlotUI[] spellSlots = new SpellSlotUI[3];

    public Animation blindAnimation;

    Dictionary<Lane, Vector3> laneVectors = new Dictionary<Lane, Vector3>()
    {
        { Lane.right, new Vector3(1.5f,0,0) },
        { Lane.mid, new Vector3(0,0,0) },
        { Lane.left, new Vector3(-1.5f,0,0) },
    };

    void Start()
    {
        health = maxHealth;

        healthBarScript.SetValue(health);
        healthBarScript.SetMaxValue(maxHealth);

        var lanePositions = playerPortraitTransform.Find("LanePositions");
        var right = lanePositions.Find("Right");
        var mid = lanePositions.Find("Mid");
        var left = lanePositions.Find("Left");

        laneVectors[Lane.right] = new Vector3(laneVectors[Lane.right].x, 0, right.position.x);
        laneVectors[Lane.mid] = new Vector3(laneVectors[Lane.mid].x, 0, mid.position.x);
        laneVectors[Lane.left] = new Vector3(laneVectors[Lane.left].x, 0, left.position.x);

        Destroy(lanePositions.gameObject);

        for (int i = 0; i < spellSlots.Length; i++)
        {
            var spellSlot = spellSlots[i];

            spellSlots[i] = Instantiate(spellSlotPrefab);
            spellSlots[i].transform.parent = spellSlotTransform;
            spellSlots[i].transform.localScale = Vector3.one;
        }
    }

    public void SetNewSkill(Skill skill)
    {
        for(int i = 0; i < spellSlots.Length; i++)
        {
            var spellSlot = spellSlots[i];
            
            if(spellSlot.skill == null)
            {
                spellSlot.SetSkill(skill);
                return;
            }
        }
    }
    
    public override void TakeDamage(HitInfo hitInfo)
    {
        if (canTakeDamage)
        {
            var damageReceived = UnitTakeDamage(hitInfo);

            //Debug.Log("Take damage: " + damage);

            if (damageReceived > 0)
            {
                if (health <= 0)
                {
                    Death();
                }
                else
                {
                    anim.SetTrigger("isHurt");
                }
            }
        }
    }

    public override void GainHealth(int gain)
    {
        if (!GameManager.instance.isGameOver && !isDead)
        {
            health += gain;

            if(health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    public void ToggleBlindStatus()
    {
        if (isBlind)
        {
            blindAnimation.Play("ShadowBlind");
        }
        else
        {
            blindAnimation.Play("ShadowUnblind");
        }
    }

    public void Death()
    {
        health = 0;
        isDead = true;
                
        anim.SetTrigger("die");

        GameManager.instance.GameOver();
    }

    public void Victory()
    {
        anim.SetTrigger("victory");
    }

    public bool canUseSkills
    {
        get
        {
            return

                !isDead
                && !GameManager.instance.isGameOver
                && !GameManager.instance.isGamePaused
                && !GameManager.instance.isMovingToNextWave;
        }
    }

    public void SetInvincibility(bool isPlayerInvincible)
    {
        isInvincible = isPlayerInvincible;

        anim.SetBool("isInvincible", isInvincible);
    }
        
    void OnTriggerEnter(Collider collision)
    {
        if (isDead) return;

        if(collision.gameObject.layer == LayerConstants.monsterLayer)
        {
            var monster = collision.GetComponent<Monster>();
            if (monster && !monster.isDead)
            {
                monster.CollideWithPlayer();
            }
        }

        /*if (collision.gameObject.layer == LayerConstants.pickupLayer)
        {
            var pickup = collision.GetComponent<PickupCube>();
            if (pickup && !pickup.isDead)
            {
                pickup.Activate(this);
            }
        }*/
    }

    void UpdateHealthBar()
    {
        /*var percentHealth = 100 * health / maxHealth;

        var lerpHealth = Mathf.Lerp(healthBar.value, percentHealth, 2 * Time.deltaTime);
        healthBar.value = lerpHealth;*/

        healthBarScript.SetValue(health);
    }

    void Update()
    {
        UpdateHealthBar();

        if (!isDead)
        {
            base.UnitUpdate();

            if (GameManager.instance.isMovingToNextWave)
            {
                var dir = new Vector3(0, 0, 1);
                transform.position += dir * speed * Time.deltaTime;
                //anim.SetFloat("Speed", speed);
            }
            else
            {
                //anim.SetFloat("Speed", 0);
            }

            //Debug.Log(destinationLane);
                        
            var destinationVector = laneVectors[destinationLane];
            var xDiff = destinationVector.x - transform.position.x;

            if (xDiff != 0)
            {
                Vector3 currentPosition = transform.position;
                Vector3 portraitPosition = playerPortraitTransform.position;

                if (Mathf.Abs(xDiff) < 0.05)
                {
                    transform.position = new Vector3(destinationVector.x, transform.position.y, transform.position.z);
                    playerPortraitTransform.position = new Vector3(destinationVector.z, portraitPosition.y, portraitPosition.z);
                    currentLane = destinationLane;
                }
                else
                {
                    var totalLaneVector = laneVectors[Lane.right] - laneVectors[Lane.left];

                    var movementCompletionPercent = (currentPosition.x - laneVectors[Lane.left].x)/ totalLaneVector.x;
                    var portraitXPos = movementCompletionPercent * totalLaneVector.z + laneVectors[Lane.left].z;
                    
                    playerPortraitTransform.position = new Vector3(portraitXPos, portraitPosition.y, portraitPosition.z);

                    currentPosition.x += Mathf.Sign(xDiff) * speed * Time.deltaTime * .5f;
                    transform.position = currentPosition;
                }
            }
        }
    }
}
