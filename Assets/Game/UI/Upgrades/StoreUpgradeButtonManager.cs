using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class StoreUpgradeButtonManager : MonoBehaviour
{
    public StoreUpgradeButton upgradeButtonPrefab;

    public UpgradeDatabase database;

    public Transform upgradeList;

    void Start()
    {
        foreach (var upgrade in database.allUpgrades)
        {
            var upgradeLevel = PlayerPrefs.GetInt("Upgrade_" + upgrade.upgradeName, 0) + 1;

            if (upgradeLevel < upgrade.maxLevel)
            {
                var newButton = Instantiate(upgradeButtonPrefab);

                newButton.InitButton(upgrade, upgradeLevel);

                newButton.transform.SetParent(upgradeList, false);
            }
        }
    }

}
