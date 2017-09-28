using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PunchCombatUI : CombatUI
{
    public GameObject onHitEffect;

    int monstersMask;

    Weapon weapon;

    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;

        GameManager.instance.touchInputManager.touchStart += OnTouchStart;

        monstersMask = LayerMask.GetMask("Monsters");
    }

    private void OnTouchStart(Touch touch)
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
        var monsterObjects = Physics.OverlapSphere(hitPos, 2f, monstersMask);
        foreach (var monsterObject in monsterObjects)
        {
            var monster = monsterObject.GetComponent<Monster>();
            if (monster)
            {
                var monsterPos = monster.transform.position;

                GameManager.instance.comboManager.IncreaseComboCount();

                var dir = (monsterPos - hitPos).normalized;
                dir.y = .15f;

                var force = dir * 1000;

                HitInfo hitInfo = new HitInfo
                {
                    hitStart = hitPos,
                    hitEnd = monsterPos,
                    force = force,
                };

                monster.Death(hitInfo);
            }
        }
    }

    public override void Destroy()
    {
        TouchInputManager.instance.touchStart -= OnTouchStart;
    }
}
