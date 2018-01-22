using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public UpgradeDatabase database;

    void Start()
    {
        PlayerPrefs.SetInt("Upgrade_PickupRate", 5);
        PlayerPrefs.SetInt("Upgrade_MovementSpeed", 8);

        foreach (var upgrade in database.allUpgrades)
        {
            var upgradeLevel = PlayerPrefs.GetInt("Upgrade_" + upgrade.upgradeName, 0);

            if(upgradeLevel > 0)
            {
                upgrade.ApplyUpgrade(upgradeLevel);
            }
        }
    }

}
