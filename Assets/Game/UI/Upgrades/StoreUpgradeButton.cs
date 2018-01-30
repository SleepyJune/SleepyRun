using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class StoreUpgradeButton : MonoBehaviour
{
    public Text titleText;
    public Text descText;
    public Image iconImage;

    public Text costText;

    Upgrade currentUpgrade;

    StoreUpgradeButtonManager upgradeManager;

    //int upgradeLevel;

    void Start()
    {
        upgradeManager = transform.parent.parent.GetComponent<StoreUpgradeButtonManager>();
    }

    public void InitButton(Upgrade upgrade, int level)
    {
        currentUpgrade = upgrade;

        var upgradeInfo = upgrade.stats[level];

        titleText.text = upgrade.name + " Lv. " + level;
        iconImage.sprite = upgrade.iconImage;
        costText.text = upgradeInfo.cost.ToString();

        if (descText)
        {
            descText.text = upgrade.description;
        }
    }

    public void OnBuyUpgrade()
    {
        upgradeManager.BuyUpgrade(currentUpgrade);
    }
}