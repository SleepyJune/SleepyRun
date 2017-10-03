using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Sprite image;

    public int damage;
    
    public CombatUI combatUI;
    public CombatUI ultimateUI;

    public float minSlashRange = 1;
    public float maxSlashRange = 10;

    public float weaponRadius = .5f;
}
