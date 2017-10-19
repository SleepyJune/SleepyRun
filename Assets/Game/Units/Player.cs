using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [System.NonSerialized]
    public bool isBossFight = false;

    void Start()
    {
        health = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        if (!GameManager.instance.isGameOver)
        {
            health -= damage;

            Debug.Log("Take damage: " + damage);

            if (health <= 0)
            {
                Death();
            }
        }
    }

    void Death()
    {
        health = 0;
        isDead = true;
                
        anim.SetTrigger("Die");

        GameManager.instance.GameOver();
    }

    void Update()
    {
        if (!isDead)
        {
            base.CheckBuffs();

            if (!isBossFight)
            {
                var dir = new Vector3(0, 0, 1);
                transform.position += dir * speed * Time.deltaTime;
                anim.SetFloat("Speed", speed);
            }
            else
            {
                anim.SetFloat("Speed", 0);
            }            
        }
    }
}
