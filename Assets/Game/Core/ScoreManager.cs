using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Analytics;

public class ScoreManager : MonoBehaviour
{
    [NonSerialized]
    public int score = 0;
    [NonSerialized]
    public int stageScore = 0;

    [NonSerialized]
    public int rewardedPoints = 0;

    [NonSerialized]
    public int totalCollected = 0;
    [NonSerialized]
    public int stageCollected = 0;

    public Text scoreText;

    public Text appleText;

    [NonSerialized]
    public int appleToCollect = 5;

    [NonSerialized]
    public ComboManager comboManager;

    [NonSerialized]
    public int stageRewardAdWatched = 0;

    [NonSerialized]
    public int stageRevivesUsed = 0;

    [NonSerialized]
    public int missedGoodApples = 0;
    [NonSerialized]
    public int stageMissedGoodApples = 0;

    [NonSerialized]
    public float stageMissedPercent = 0.0f;

    public Animator appleCountAC;

    void Start()
    {
        comboManager = GetComponent<ComboManager>();

        GameManager.instance.stageEventManager.OnStageResetEvent += OnStageReset;

        UpdateScoreText();
    }

    void OnStageReset()
    {
        stageCollected = 0;
        stageScore = 0;
        stageRewardAdWatched = 0;
        stageMissedGoodApples = 0;

        UpdateScoreText();
    }

    public void SetNewLevelStats(bool levelComplete = true)
    {
        //var points = totalCollected;//Math.Max(0, totalCollected - GameManager.instance.monsterManager.GetMissedCount());

        if (!levelComplete)
        {
            GameManager.instance.scoreManager.SetNewStageStats(false);
        }

        LevelStats stats = new LevelStats
        {
            levelComplete = levelComplete,
            currentLevel = GameManager.instance.stageEventManager.currentStageCount,
            levelTime = GameManager.instance.timerManager.levelTime,
            time = GameManager.instance.timerManager.totalGameTime,
            monstersKilled = GameManager.instance.monsterManager.GetTotalKillCount(),
            monstersMissedCount = missedGoodApples,
            totalMonsterSpawned = GameManager.instance.monsterManager.GetMonsterSpawnCount(MonsterCollisionMask.All),
            totalGoodMonsterSpawned = GameManager.instance.monsterManager.GetMonsterSpawnCount(MonsterCollisionMask.Good),
            monstersCollected = totalCollected,
            points = score,
            collectedPoints = score - rewardedPoints,
            moneyEarned = (int)Math.Floor((float)score / 100),
            rewardAdPoints = rewardedPoints,
            rewardAdWatched = GameManager.instance.adManager.rewardAdWatched,
            revivesUsed = GameManager.instance.revivesUsed,
        };

        var statsDictionary = stats.ParseDictionary();
        var results = Analytics.CustomEvent("gameOver2", statsDictionary);

        Debug.Log("Sending Analytics: " + results);

        Analytics.FlushEvents();

        SceneChanger.levelStats = stats;
    }

    public void SetNewStageStats(bool levelComplete = true)
    {
        //var points = totalCollected;//Math.Max(0, totalCollected - GameManager.instance.monsterManager.GetMissedCount());

        //Debug.Log(GameManager.instance.monsterManager.GetStageGoodMonsterSpawnCount());

        var stageGoodApples = stageMissedGoodApples + stageCollected;

        var collectPercent = stageGoodApples > 0 ? (float)stageCollected / stageGoodApples : 0;
        Debug.Log(collectPercent);

        StageStats stats = new StageStats
        {
            levelComplete = levelComplete,
            currentLevel = GameManager.instance.stageEventManager.currentStageCount,
            monstersKilled = GameManager.instance.monsterManager.GetKillCount(),
            stageMissedGoodApples = stageMissedGoodApples,
            appleCollectPercent = collectPercent,
            monstersCollected = stageCollected,
            stagePoints = stageScore,
            rewardAdWatched = stageRewardAdWatched,
            revivesUsed = stageRevivesUsed,
        };

        var statsDictionary = stats.ParseDictionary();
        var results = Analytics.CustomEvent("stageOver2", statsDictionary);

        Debug.Log("Sending Analytics: " + results);
    }

    public void UpdateScoreText()
    {
        scoreText.text = score.ToString();
        //appleText.text = stageCollected.ToString() + " / " + appleToCollect;

        appleText.text = totalCollected.ToString();
    }

    public void AddCollectedMonsterCount(int collected = 1)
    {
        totalCollected += collected;
        stageCollected += collected;

        float stageMultiplier = 1+(GameManager.instance.stageEventManager.currentStageCount / 30.0f);
        var amount = (int)Math.Round(collected * comboManager.comboCount * stageMultiplier);

        AddScore(amount);
    }

    public void AddScoreOnHit(HitInfo hitInfo)
    {
        //score += (int)Mathf.Max(1,Mathf.Round(comboManager.comboCount * hitInfo.damage / 100f));
        //UpdateScoreText();
    }

    public void AddScoreOnMonsterKill(Monster monster)
    {
        //score += monster.maxHealth;
        //UpdateScoreText();
    }

    public void AddRewardPoints(int amount)
    {
        rewardedPoints += amount;
        AddScore(amount);
    }

    public void AddScore(int amount)
    {
        score += amount;
        stageScore += amount;

        UpdateScoreText();
    }

    public void AddMissedGoodApple(int amount)
    {
        stageMissedGoodApples += amount;
        missedGoodApples += amount;

        var stageGoodApple = GameManager.instance.monsterManager.stageGoodMonsterSpawn;
                
        stageMissedPercent = stageGoodApple > 0 ? (float)stageMissedGoodApples / stageGoodApple : 0.0f;

        if(stageMissedPercent > .5f)
        {
            appleCountAC.SetBool("FlashRed", true);
        }
        else
        {
            appleCountAC.SetBool("FlashRed", false);
        }
    }
}