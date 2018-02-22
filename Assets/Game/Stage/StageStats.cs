using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StageStats
{
    public bool levelComplete;
    public int currentLevel;
    public int monstersKilled;
    public int stagePoints;
    public int stageMissedGoodApples;
    public float appleCollectPercent;

    public int monstersCollected;

    public int revivesUsed;
    public int rewardAdWatched;

    public Dictionary<string, object> ParseDictionary() //max 10 parameters
    {
        return new Dictionary<string, object>
        {
            {"level", currentLevel },
            {"levelComplete", levelComplete },
            {"monstersKilled", monstersKilled},
            {"stagePoints", stagePoints },
            {"appleCollectPercent", appleCollectPercent },
            {"monstersCollected", monstersCollected },
            {"revivesUsed" , revivesUsed },
            {"rewardAdWatched", rewardAdWatched},
        };
    }
}
