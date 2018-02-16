using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class StoreUpgradeButtonManager : MonoBehaviour
{
    public StoreUpgradeButton upgradeButtonPrefab;

    public UpgradeDatabase database;

    public Transform upgradeList;

    public Text coinText;

    Dictionary<Upgrade, StoreUpgradeButton> buttonCache = new Dictionary<Upgrade, StoreUpgradeButton>();

    double mockCoins = 8 * Math.Pow(10, 4);

    void Start()
    {
        InitializeButtons();
    }

    public void InitializeButtons()
    {
        buttonCache = new Dictionary<Upgrade, StoreUpgradeButton>();

        foreach (var upgrade in database.allUpgrades)
        {
            var upgradeLevel = PlayerPrefs.GetInt("Upgrade_" + upgrade.upgradeName, 0) + 1;

            if (upgradeLevel < upgrade.maxLevel)
            {
                var newButton = Instantiate(upgradeButtonPrefab);

                newButton.InitButton(upgrade, upgradeLevel);

                newButton.transform.SetParent(upgradeList, false);

                buttonCache.Add(upgrade, newButton);
            }
        }

        coinText.text = mockCoins.ToString();
    }

    public void BuyUpgrade(Upgrade upgrade)
    {
        var upgradeLevel = PlayerPrefs.GetInt("Upgrade_" + upgrade.upgradeName, 0) + 1;

        var upgradeInfo = upgrade.stats[upgradeLevel];
        var cost = upgradeInfo.cost;

        if(mockCoins >= cost)
        {
            PlayerPrefs.SetInt("Upgrade_" + upgrade.upgradeName, upgradeLevel);

            var results = Analytics.CustomEvent("buyUpgrade", new Dictionary<string, object>()
            {
                { "upgradeName", upgrade.upgradeName},
                { "level", upgradeLevel},
                { "cost", cost},
            });

            Debug.Log("Sending Analytics: " + results);

            ReInitializeButton(upgrade);

            mockCoins -= cost;
            coinText.text = mockCoins.ToString();            
        }

        
    }

    public void ReInitializeButton(Upgrade upgrade)
    {
        StoreUpgradeButton button;

        if (buttonCache.TryGetValue(upgrade, out button))
        {
            var upgradeLevel = PlayerPrefs.GetInt("Upgrade_" + upgrade.upgradeName, 0) + 1;

            if (upgradeLevel < upgrade.maxLevel)
            {
                button.InitButton(upgrade, upgradeLevel);
            }
            else
            {
                Destroy(button.gameObject);
            }
        }
    }

    public void ReInitializeButtons()
    {
        upgradeList.DestroyChildren();

        InitializeButtons();
    }

}
