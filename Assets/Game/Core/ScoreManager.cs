﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [NonSerialized]
    public int score = 0;

    [NonSerialized]
    public int totalCollected = 0;

    public Text scoreText;

    public Text appleText;

    [NonSerialized]
    public ComboManager comboManager;

    void Start()
    {
        comboManager = GetComponent<ComboManager>();

        UpdateScoreText();
    }

    public void SetNewLevelStats(bool levelComplete = true)
    {
        //var points = totalCollected;//Math.Max(0, totalCollected - GameManager.instance.monsterManager.GetMissedCount());

        LevelStats stats = new LevelStats
        {
            levelComplete = levelComplete,
            levelTime = GameManager.instance.timerManager.levelTime,
            time = GameManager.instance.timerManager.totalGameTime,
            monstersKilled = GameManager.instance.monsterManager.GetKillCount(),
            monstersMissedCount = GameManager.instance.monsterManager.GetMissedCount(),
            totalMonsterSpawned = GameManager.instance.monsterManager.GetMonsterSpawnCount(MonsterCollisionMask.All),
            totalGoodMonsterSpawned = GameManager.instance.monsterManager.GetMonsterSpawnCount(MonsterCollisionMask.Good),
            monstersCollected = totalCollected,
            points = score,
        };

        SceneChanger.levelStats = stats;
    }

    public void UpdateScoreText()
    {
        scoreText.text = score.ToString();
        appleText.text = totalCollected.ToString();
    }

    public void AddCollectedMonsterCount(int collected = 1)
    {
        float stageMultiplier = 1+(GameManager.instance.stageEventManager.currentStageCount / 30.0f);

        Debug.Log(stageMultiplier);

        totalCollected += collected;
        score += (int)Math.Round(collected * comboManager.comboCount * stageMultiplier);
        UpdateScoreText();
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

    public void AddScore(int amount)
    {
        //score += amount;
        //UpdateScoreText();
    }
}