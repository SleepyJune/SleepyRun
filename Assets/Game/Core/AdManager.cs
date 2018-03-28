using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;

public class AdManager : MonoBehaviour
{
    public AdRewardWindow adRewardWindow;

    private int promisedReward = 500;

    public int rewardAdWatched = 0;

    public string adVideoId = "adVideo";

    public bool isAdReady()
    {
        return Advertisement.IsReady(adVideoId);
    }

    public bool ShowAdForReward(int reward)
    {
        if (Advertisement.IsReady(adVideoId))
        {
            promisedReward = reward;

            var options = new ShowOptions { resultCallback = RewardHandleShowResult };
            Advertisement.Show(adVideoId, options);

            return true;
        }

        return false;
    }

    private void RewardHandleShowResult(ShowResult result)
    {        
        if (CheckAdResult(result))
        {
            GameManager.instance.scoreManager.AddRewardPoints(promisedReward);
            GameManager.instance.scoreManager.stageRewardAdWatched += 1;
            rewardAdWatched += 1;
        }
    }

    public bool ShowAdForRevive()
    {
        if (Advertisement.IsReady(adVideoId))
        {
            var options = new ShowOptions { resultCallback = ReviveHandleShowResult };
            Advertisement.Show(adVideoId, options);

            return true;
        }

        return false;
    }

    private void ReviveHandleShowResult(ShowResult result)
    {
        if (CheckAdResult(result))
        {
            GameManager.instance.ReviveAdComplete();
        }
    }

    private bool CheckAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

                SendAdEvent(1, 0, 0);
                return true;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");

                SendAdEvent(0, 1, 0);
                return true;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");

                SendAdEvent(0, 0, 1);
                break;
        }

        return false;
    }

    private void SendAdEvent(int finished, int skipped, int failed)
    {
        Analytics.CustomEvent("adWatched", new Dictionary<string, object>()
        {
            { "started", 1 },
            { "finished", finished },
            { "skipped", skipped },
            { "failed", failed},
        });
    }
}
