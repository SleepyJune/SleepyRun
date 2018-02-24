using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Player : Unit
{
    [System.NonSerialized]
    public bool isBossFight = false;

    //public float beltSpeed = 2.0f;

    public Lane destinationLane = Lane.mid;
    [System.NonSerialized]
    public Lane currentLane = Lane.mid;

    //public UIUnitFrame_Bar healthBarScript;
    //public Slider healthBarSlider;

    public Transform heartHolder;
    public HeartPrefabScript heartPrefab;
    List<HeartPrefabScript> heartSlots = new List<HeartPrefabScript>();

    public WeaponButton playerPortrait;
    public EffectOverlayManager overlayManager;
    public Transform playerPortraitTransform;

    //[System.NonSerialized]
    //public Skill[] skills = new Skill[3];

    public SpellSlotUI spellSlotPrefab;
    public Transform spellSlotTransform;

    public int numSpellSlots = 3;
    SpellSlotUI[] spellSlots = new SpellSlotUI[3];

    public Animation blindAnimation;

    public StatusBuffObject reviveBuffObj;

    Dictionary<Lane, Vector3> laneVectors = new Dictionary<Lane, Vector3>()
    {
        { Lane.right, new Vector3(1.5f,0,0) },
        { Lane.mid, new Vector3(0,0,0) },
        { Lane.left, new Vector3(-1.5f,0,0) },
    };

    void Start()
    {
        Invoke("InitPlayer", 0.25f);
    }

    void InitPlayer()
    {
        health = maxHealth;
        
        UpdateHealthBar();

        var lanePositions = playerPortraitTransform.Find("LanePositions");
        var right = lanePositions.Find("Right");
        var mid = lanePositions.Find("Mid");
        var left = lanePositions.Find("Left");

        laneVectors[Lane.right] = new Vector3(laneVectors[Lane.right].x, 0, right.position.x);
        laneVectors[Lane.mid] = new Vector3(laneVectors[Lane.mid].x, 0, mid.position.x);
        laneVectors[Lane.left] = new Vector3(laneVectors[Lane.left].x, 0, left.position.x);

        Destroy(lanePositions.gameObject);

        spellSlots = new SpellSlotUI[numSpellSlots];

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
            if(hitInfo.damage > 0)
            {
                hitInfo.damage = 1;
            }
                        
            var damageReceived = UnitTakeDamage(hitInfo);

            //Debug.Log("Take damage: " + damage);

            if (damageReceived > 0)
            {
                GameManager.instance.comboManager.BreakCombo();

                UpdateHealthBar();

                if (health <= 0)
                {
                    Death();
                }
                else
                {
                    SelfBuff(reviveBuffObj);
                    anim.SetTrigger("isHurt");
                }
            }
        }
    }

    public override void GainShield(int shieldAmount)
    {
        if (!GameManager.instance.isGameOver && !isDead)
        {
            if (shieldAmount > 0)
            {
                shield += shieldAmount;
                shield = Mathf.Min(health, shield);                
            }

            UpdateHealthBar();
        }
    }

    public override void GainHealth(int gain)
    {
        if (!GameManager.instance.isGameOver && !isDead)
        {
            health += 1;//gain;

            if(health > maxHealth)
            {
                health = maxHealth;
            }

            UpdateHealthBar();
        }
    }

    public void Death()
    {
        health = 0;
        isDead = true;
                
        anim.SetTrigger("die");

        GameManager.instance.GameOver();
    }

    public void Revive()
    {
        isDead = false;
        health = maxHealth;
        UpdateHealthBar();

        SelfBuff(reviveBuffObj);

        anim.SetTrigger("revive");
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
                && !GameManager.instance.isMovingToNextWave
                && Time.timeScale > 0.1f;
        }
    }

    public void OnStatusChange(StatusEffectType statusType, bool status)
    {
        if (statusType == StatusEffectType.Rooted)
        {

        }
        else if (statusType == StatusEffectType.Blinded)
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
        else if (statusType == StatusEffectType.Invincible)
        {
            //anim.SetBool("isInvincible", isInvincible);

            if (status)
            {
                overlayManager.AddEffectOverlay("InvincibleEffect");
            }
            else
            {
                var overlay = overlayManager.GetEffectOverlay("InvincibleEffect");

                if (overlay != null)
                {
                    var animator = overlay.GetComponent<Animator>();

                    if (animator)
                    {
                        animator.SetTrigger("Die");
                    }
                }

                overlayManager.RemoveEffectOverlay("InvincibleEffect", false);
            }
        }
        else if (statusType == StatusEffectType.Silenced)
        {
            for (int i = 0; i < spellSlots.Length; i++)
            {
                var spellSlot = spellSlots[i];
                spellSlot.DisableSkill(isSilenced);
            }
        }
        else if (statusType == StatusEffectType.Confused)
        {
            if (status)
            {
                overlayManager.AddEffectOverlay("ConfusedAnimation");
            }
            else
            {
                overlayManager.RemoveEffectOverlay("ConfusedAnimation");
            }
        }
        else
        {

        }
    }
        
    void OnTriggerEnter(Collider collision)
    {
        if (isDead) return;

        if (GameManager.instance.isGameOver
            || GameManager.instance.isMovingToNextWave
            || GameManager.instance.isGamePaused)
        {
            return;
        }

        if (collision.gameObject.layer == LayerConstants.monsterLayer)
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

        var lerpHealth = Mathf.Lerp(healthBarSlider.value, percentHealth, 2 * Time.deltaTime);
        healthBarSlider.value = lerpHealth;*/

        var diff = health - heartHolder.childCount;
                
        if (diff > 0)
        {
            for (int i = 0;i< diff; i++)
            {
                var newHeart = Instantiate(heartPrefab);
                newHeart.transform.SetParent(heartHolder, false);

                heartSlots.Add(newHeart);
            }
        }
        else if(diff < 0)
        {
            
            for (int i = 0; i < -diff; i++)
            {
                if(heartSlots.Count == 0)
                {
                    break;
                }

                var heart = heartSlots[heartSlots.Count-1];
                heartSlots.RemoveAt(heartSlots.Count - 1);
                Destroy(heart.gameObject);
            }
        }

        int shieldLoop = shield;

        for (int i = 0; i < heartSlots.Count; i++)
        {
            var heart = heartSlots[i];

            heart.SetIcon(shieldLoop > 0);
            shieldLoop--;
        }

        //healthBarScript.SetValue(health);
    }

    void Update()
    {        
        if (!isDead)
        {
            base.UnitUpdate();

            if (GameManager.instance.isMovingToNextWave)
            {                
                //anim.SetFloat("Speed", speed);
            }
            else
            {
                //anim.SetFloat("Speed", 0);
            }

            //var dir = new Vector3(0, 0, 1);
            //transform.position += dir * beltSpeed * Time.deltaTime;

            //Debug.Log(destinationLane);

            var destinationVector = laneVectors[destinationLane];
            var xDiff = destinationVector.x - transform.position.x;

            if (xDiff != 0)
            {
                Vector3 currentPosition = transform.position;
                Vector3 portraitPosition = playerPortraitTransform.position;

                if (Mathf.Abs(xDiff) < 0.05 || Mathf.Abs(xDiff) <= speed * Time.deltaTime)
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
