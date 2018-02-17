using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteScript : MonoBehaviour
{
    LevelStats stats;

    public Text pointText;
    public Text missedCountText;
    public Text killCountText;
    public Text timeText;
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

        pointText.text = stats.monstersCollected.ToString();
        missedCountText.text = stats.monstersMissedCount.ToString();
        killCountText.text = stats.monstersKilled.ToString();

        string minSec = string.Format("{0}:{1:00}", (int)stats.time / 60, (int)stats.time % 60);
        timeText.text = minSec;

        earningText.text = "$" + stats.points.ToString();

        appleEndingAC.Play(GetAppleEmote(stats));

        SetGold();
    }

    private void SetGold()
    {
        MoneyManager.instance.IncreaseGold(stats.points);
    }

    string GetAppleEmote(LevelStats stats)
    {
        int totalMonsters = stats.totalMonsterSpawned;
        float earningPercent = (float)stats.monstersCollected / stats.totalGoodMonsterSpawned; //divide by 0??
        float killPercent = (float)stats.monstersKilled / totalMonsters;
        float levelTimePercent = stats.time / stats.levelTime;

        Debug.Log("Apples Spawned: " + stats.totalGoodMonsterSpawned);

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

        if (earningPercent <= .1f)
        {
            return "lines";
        }
        else if (earningPercent <= .2f)
        {
            return "no_words";
        }
        else if (earningPercent <= .3f)
        {
            return "laugh2";
        }
        else if (earningPercent <= .4f)
        {
            return "worm";
        }
        else if (earningPercent <= .5f)
        {
            return "half_eaten";
        }
        else if (earningPercent >= .5f)
        {
            return "sunglasses";
        }

        return "sliced";
    }
}
