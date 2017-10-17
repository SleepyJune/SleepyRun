using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PunchCombatUI : CombatUI
{
    public GameObject onHitEffect;
    
    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;        
    }

    public override void OnTouchStart(Touch touch)
    {
        var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

        if (onHitEffect)
        {
            Instantiate(onHitEffect, pos, Quaternion.identity);
        }

        DestroyMonsters(pos);
    }

    void DestroyMonsters(Vector3 hitPos)
    {
        var monsterObjects = Physics.OverlapSphere(hitPos, 2f, LayerConstants.monsterMask);
        foreach (var monsterObject in monsterObjects)
        {
            var monster = monsterObject.GetComponent<Monster>();
            if (monster)
            {
                var monsterPos = monster.transform.position;
                
                var dir = (monsterPos - hitPos).normalized;
                dir.y = .15f;

                var force = dir * 1000;

                HitInfo hitInfo = new HitInfo
                {
                    hitStart = hitPos,
                    hitEnd = monsterPos,
                    force = force,
                    damage = weapon.damage
                };

                monster.TakeDamage(hitInfo);
            }
        }
    }

    public override void End()
    {

    }
}
