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
    public Text levelText;
    public Image iconImage;

    public Text costText;
    public Button buyButton;

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

        titleText.text = upgrade.name;
        iconImage.sprite = upgrade.iconImage;
        costText.text = upgradeInfo.cost.ToString();
        levelText.text = "Lv." + level;

        if (descText)
        {
            descText.text = upgrade.description;
        }

        SetButtonInteractive(upgradeInfo.cost);
    }

    public void OnBuyUpgrade()
    {
        upgradeManager.OpenConfirmWindow(currentUpgrade);
    }

    public void SetButtonInteractive(int cost)
    {
        if (MoneyManager.instance.GetGold() >= cost)
        {
            if (!buyButton.interactable)
            {
                buyButton.interactable = true;
            }
        }
        else
        {
            buyButton.interactable = false;
        }
    }
}