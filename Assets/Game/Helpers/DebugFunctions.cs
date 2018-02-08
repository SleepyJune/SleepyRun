using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class DebugFunctions : MonoBehaviour
{
    public UpgradeDatabase upgradeDatabase;
    public StoreUpgradeButtonManager upgradeManager;
    
    public void ResetAllUpgrades()
    {
        if (upgradeDatabase)
        {
            PlayerPrefs.DeleteAll();

            foreach(var upgrade in upgradeDatabase.allUpgrades)
            {
                if(upgrade != null)
                {
                    PlayerPrefs.SetInt("Upgrade_" + upgrade.upgradeName, 0);
                }
            }

            upgradeManager.ReInitializeButtons();
        }
    }
}
