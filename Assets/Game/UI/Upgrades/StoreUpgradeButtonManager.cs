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

    public StoreUpgradeConfirmWindow confirmWindow;

    Dictionary<Upgrade, StoreUpgradeButton> buttonCache = new Dictionary<Upgrade, StoreUpgradeButton>();
        
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

        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        coinText.text = MoneyManager.instance.GetGold().ToString();
    }

    public void OpenConfirmWindow(Upgrade upgrade)
    {
        confirmWindow.OpenWindow(upgrade);
    }

    public void BuyUpgrade(Upgrade upgrade)
    {
        var upgradeLevel = PlayerPrefs.GetInt("Upgrade_" + upgrade.upgradeName, 0) + 1;

        var upgradeInfo = upgrade.stats[upgradeLevel];
        var cost = upgradeInfo.cost;

        if(MoneyManager.instance.GetGold() >= cost)
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

            MoneyManager.instance.DecreaseGold(cost);
            UpdateCoinText();
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
