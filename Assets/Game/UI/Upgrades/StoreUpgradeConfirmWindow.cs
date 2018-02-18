using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class StoreUpgradeConfirmWindow : CanvasGroupWindow
{
    public AudioClip coinSound;
    public AudioSource uiAudioSource;
    
    public StoreUpgradeButtonManager upgradeManager;

    Upgrade currentUpgrade;

    public void OpenWindow(Upgrade upgrade)
    {
        if (isWindowOpen)
        {
            HideWindow();
        }
        else
        {
            currentUpgrade = upgrade;
            ShowWindow();
        }        
    }

    public void Confirm()
    {
        upgradeManager.BuyUpgrade(currentUpgrade);
        uiAudioSource.PlayOneShot(coinSound);
        HideWindow();
    }
}
