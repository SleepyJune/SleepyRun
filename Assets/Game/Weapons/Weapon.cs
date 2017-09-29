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

    public int minSlashRange = 1;
    public int maxSlashRange = 10;    
}
