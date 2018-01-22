using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Database")]
public class UpgradeDatabase : ScriptableObject
{
    public Upgrade[] allUpgrades = new Upgrade[0];
}
