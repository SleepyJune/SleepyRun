using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Player : Unit
{
    [System.NonSerialized]
    public bool isBossFight = false;

    public Lane destinationLane = Lane.mid;

    //public Slider healthBar;

    public UIUnitFrame_Bar healthBarScript;

    public WeaponButton playerPortrait;

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
    }
    
    public void TakeDamage(int damage)
    {
        if (!GameManager.instance.isGameOver)
        {
            health -= damage;

            //Debug.Log("Take damage: " + damage);

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

    public void GainHealth(int gain)
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
            base.CheckBuffs();

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

                if (Mathf.Abs(xDiff) < 0.05)
                {
                    transform.position = new Vector3(destinationVector.x, transform.position.y, transform.position.z);
                }
                else
                {
                    currentPosition.x += Mathf.Sign(xDiff) * speed * Time.deltaTime * .5f;
                    transform.position = currentPosition;
                }
            }
        }
    }
}
