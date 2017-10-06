using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ProjectileCombatUI : SlashCombatUI
{
    public GameObject projectilePrefab;

    protected override void ExecuteAttack(LineRenderer line)
    {
        CreateProjectile(line);
    }

    void CreateProjectile(LineRenderer line)
    {
        var projectileObject = Instantiate(projectilePrefab.gameObject, line.GetPosition(0), Quaternion.identity);
        var projectile = projectileObject.GetComponent<LinearSpell>();

        projectile.start = line.GetPosition(0);
        projectile.end = line.GetPosition(1);
        //projectile.speed = (projectile.end - projectile.start).magnitude * 300;

        //projectile.SetVelocity();
    }    
}
