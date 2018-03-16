using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteScript : MonoBehaviour
{
    LevelStats stats;

    public Text collectedText;
    public Text pointsText;
    public Text earningText;

    public Animator appleEndingAC;

    void Start()
    {
        stats = SceneChanger.levelStats;

        if (stats == null)
        {
            //SceneChanger.ChangeScene("IntroScreen2");
            return;
        }
                
        appleEndingAC.Play(GetAppleEmote(stats));

        SetGold();
    }

    private void SetGold()
    {
        MoneyManager.instance.IncreaseGold(stats.moneyEarned);
    }

    string GetAppleEmote(LevelStats stats)
    {
        int totalMonsters = stats.totalMonsterSpawned;

        var totalGoodApples = stats.monstersCollected + stats.monstersMissedCount;

        float earningPercent = totalGoodApples > 0 ? (float)stats.monstersCollected / totalGoodApples : 0; //divide by 0??
        float killPercent = (float)stats.monstersKilled / totalMonsters;
        float levelTimePercent = stats.time / stats.levelTime;

        collectedText.text = stats.monstersCollected.ToString();
        pointsText.text = stats.points.ToString();

        int hourlyRate = (int)Math.Round(stats.moneyEarned / Math.Ceiling(stats.time / 60));

        //killCountText.text = "$" + hourlyRate.ToString() + "/hr";
        //string minSec = string.Format("{0}:{1:00}", (int)stats.time / 60, (int)stats.time % 60);
        //timeText.text = stats.currentLevel.ToString();// minSec;

        earningText.text = "$" + stats.moneyEarned.ToString();

        Debug.Log("Collection: " + ((int)Math.Round(earningPercent * 100)).ToString() + "%");

        /*if(!stats.levelComplete && levelTimePercent <= .5f)
        {
            return "lines";
        }
        else if (!stats.levelComplete)
        {
            return "no_words";
        }*/


        if (killPercent > .8f)
        {
            return "sliced";
        }

        if (earningPercent <= .3f)
        {
            return "lines";
        }
        else if (earningPercent <= .4f)
        {
            return "no_words";
        }
        else if (earningPercent <= .5f)
        {
            return "laugh2";
        }
        else if (earningPercent <= .6f)
        {
            return "worm";
        }
        else if (earningPercent <= .7f)
        {
            return "half_eaten";
        }
        else if (earningPercent >= .8f)
        {
            return "sunglasses";
        }

        return "sliced";
    }
}
