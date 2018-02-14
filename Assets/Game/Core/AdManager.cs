using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    public AdRewardWindow adRewardWindow;

    private int promisedReward = 500;       
    
    public bool isAdReady()
    {
        return Advertisement.IsReady("rewardedVideo");
    }

    public bool ShowAdForReward(int reward)
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            promisedReward = reward;

            var options = new ShowOptions { resultCallback = RewardHandleShowResult };
            Advertisement.Show("rewardedVideo", options);

            return true;
        }

        return false;
    }

    private void RewardHandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

                GameManager.instance.scoreManager.AddScore(promisedReward);

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    public bool ShowAdForRevive()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = ReviveHandleShowResult };
            Advertisement.Show("rewardedVideo", options);

            return true;
        }

        return false;
    }

    private void ReviveHandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

                GameManager.instance.ReviveAdComplete();

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}
