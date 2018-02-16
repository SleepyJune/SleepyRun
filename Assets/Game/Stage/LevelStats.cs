using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LevelStats
{
    public bool levelComplete;
    public int currentLevel;
    public float levelTime;
    public float time;
    public int monstersKilled;
    public int points;
    public int collectedPoints;
    public int monstersMissedCount;
    public int monstersCollected;
    public int totalMonsterSpawned;
    public int totalGoodMonsterSpawned;

    public int revivesUsed;
    public int rewardAdPoints;
    public int rewardAdWatched;

    public Dictionary<string, object> ParseDictionary() //max 10 parameters
    {
        return new Dictionary<string, object>
        {
            {"time", time },
            {"level", currentLevel },
            {"monstersKilled", monstersKilled},
            {"points", points },
            {"collectedPoints", collectedPoints },
            {"monstersCollected", monstersCollected },
            {"totalMonsterSpawned", totalMonsterSpawned },
            {"revivesUsed" , revivesUsed },
            {"rewardAdPoints", rewardAdPoints },
            {"rewardAdWatched", rewardAdWatched},
        };
    }
}
