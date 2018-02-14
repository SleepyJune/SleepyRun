using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class AdRewardWindow : MonoBehaviour
{
    public Text rewardText;

    public int promisedReward;

    public void SetReward(int stageLevel)
    {
        int reward = 500 + (stageLevel / 5) * 250;

        promisedReward = reward;
        rewardText.text = reward.ToString();
    }

    public void ShowAd()
    {
        GameManager.instance.adManager.ShowAdForReward(promisedReward);
    }
}
