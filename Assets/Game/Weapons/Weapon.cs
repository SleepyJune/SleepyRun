using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public enum WeaponType
{
    Dagger,
    Sword,
    Axe,
    Katana
}

public class Weapon : MonoBehaviour
{
    public Sprite image;

    public WeaponType weaponType;

    public float thrustModifier;
    public float chopModifier;
    public float slashModifier;

    public int damage;

    public CombatUI combatUI;
    public CombatUI ultimateUI;

    public float minSlashRange = 1;
    public float maxSlashRange = 10;

    public float weaponRadius = .5f;

    public float attackFrequency = 1f;

    public int staminaCost = 20;

    [NonSerialized]
    public float lastAttackTime;

    public Gradient GetSlashGradient(Vector3 start, Vector3 end)
    {
        var diff = (end - start);
        float length = diff.magnitude;

        var thrustDamage = Math.Max(0, diff.z) * (1 + thrustModifier);
        var chopDamage = Math.Max(0, -diff.z) * (1 + chopModifier);
        var slashDamage = Math.Abs(diff.x) * (1 + slashModifier);


        var estimateMaxDamage = maxSlashRange * (1 + Math.Max(Math.Max(thrustModifier, chopModifier), slashModifier));

        //var rangeDamageReduction

        var damage = thrustDamage + chopDamage + slashDamage;

        var damagePercent = damage / estimateMaxDamage;

        Debug.Log(damagePercent);

        Color slashColor = Color.white;
        if (damagePercent > 1) //75 percentile
        {
            slashColor = Color.red;
        }
        else if (damagePercent > .5)
        {
            slashColor = Color.yellow;
        }
                
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(slashColor, 0.0f),
                new GradientColorKey(slashColor, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 1.0f) }
            );

        return gradient;
    }
}
