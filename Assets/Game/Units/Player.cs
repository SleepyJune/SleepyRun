using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{   
    void Start()
    {
        health = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Take damage: " + damage);

        if(health <= 0)
        {
            Death();
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
            var dir = new Vector3(0, 0, 1);
            transform.position += dir * speed * Time.deltaTime;
            anim.SetFloat("Speed", speed);
        }
    }
}
