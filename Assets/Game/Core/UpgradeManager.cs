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
        foreach (var upgrade in database.allUpgrades)
        {
            var upgradeLevel = PlayerPrefs.GetInt("Upgrade_" + upgrade.upgradeName, 0);

            if(upgradeLevel > 0)
            {
                upgrade.ApplyUpgrade(upgradeLevel);
            }
            else if(upgrade.defaultMockLevel > 0)
            {
                upgrade.ApplyUpgrade(upgrade.defaultMockLevel);
            }
        }
    }

}
