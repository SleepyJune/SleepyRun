using System;
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

    [NonSerialized]
    public ComboManager comboManager;

    void Start()
    {
        comboManager = GetComponent<ComboManager>();

        UpdateScoreText();
    }

    public void SetNewLevelStats(bool levelComplete = true)
    {
        var points = levelComplete ? Math.Max(0, totalCollected - GameManager.instance.monsterManager.GetMissedCount()) : 0;

        LevelStats stats = new LevelStats
        {
            levelComplete = levelComplete,
            levelTime = GameManager.instance.timerManager.levelTime,
            time = GameManager.instance.timerManager.totalGameTime,
            monstersKilled = GameManager.instance.monsterManager.GetKillCount(),
            monstersMissedCount = GameManager.instance.monsterManager.GetMissedCount(),
            monstersCollected = totalCollected,
            points = points,
        };

        SceneChanger.levelStats = stats;
    }

    public void UpdateScoreText()
    {
        //scoreText.text = score.ToString();
        scoreText.text = totalCollected.ToString();
    }

    public void AddCollectedMonsterCount(int collected = 1)
    {        
        totalCollected += collected;
        UpdateScoreText();
    }

    public void AddScoreOnHit(HitInfo hitInfo)
    {
        score += (int)Mathf.Max(1,Mathf.Round(comboManager.comboCount * hitInfo.damage / 100f));
        UpdateScoreText();
    }

    public void AddScoreOnMonsterKill(Monster monster)
    {
        score += monster.maxHealth;
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }
}