﻿using System;
using UnityEngine;

public class Monster : Unit
{
    MonsterShatter shatterScript;

    [NonSerialized]
    public new Rigidbody rigidbody;

    void Awake()
    {
        shatterScript = GetComponent<MonsterShatter>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void RemoveFromStage()
    {
        if (!isDead)
        {
            GameManager.instance.comboManager.BreakCombo();
        }
    }

    public void Death(Vector3 force)
    {
        if (!isDead)
        {
            isDead = true;

            anim.SetTrigger("Die");
            anim.SetBool("isDead", true);

            //DelayAction.Add(()=>shatterScript.MakeShattered(anim.transform),.5f);
            shatterScript.MakeShattered(force);

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
