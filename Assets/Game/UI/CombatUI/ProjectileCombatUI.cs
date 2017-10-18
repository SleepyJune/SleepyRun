using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ProjectileCombatUI : CombatUI
{
    public GameObject projectilePrefab;

    int fingerID = -999;

    Vector3 start;
    Vector3 end;

    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public override void OnTouchStart(Touch touch)
    {
        if (fingerID == -999)
        {            
            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

            fingerID = touch.fingerId;

            start = pos;
            //start.z = GameManager.instance.player.transform.z

            weapon.lastAttackTime = Time.time;
        }
    }

    public override void OnTouchEnd(Touch touch)
    {
        if (fingerID == touch.fingerId)
        {
            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);
            end = pos;

            CreateProjectile(start, end);

            fingerID = -999;
        }
    }

    void CreateProjectile(Vector3 start, Vector3 end)
    {
        var projectileObject = Instantiate(projectilePrefab.gameObject, start, Quaternion.identity);
        var projectile = projectileObject.GetComponent<LinearSpell>();

        projectile.start = start;
        projectile.end = end;
        //projectile.speed = (projectile.end - projectile.start).magnitude * 300;

        //projectile.SetVelocity();
    }    
}
