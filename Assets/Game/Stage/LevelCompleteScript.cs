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
    }

    string GetAppleEmote(LevelStats stats)
    {
        int totalMonsters = stats.monstersCollected + stats.monstersKilled + stats.monstersMissedCount;
        float earningPercent = stats.monstersCollected / (totalMonsters - stats.monstersKilled);
        float killPercent = stats.monstersKilled / totalMonsters;
        float levelTimePercent = stats.time / stats.levelTime;

        if(!stats.levelComplete && levelTimePercent <= .5f)
        {
            return "lines";
        }
        else if (!stats.levelComplete)
        {
            return "no_words";
        }


        if (killPercent > .8f)
        {
            return "sliced";
        }

        if (earningPercent <= .2f)
        {
            return "puke";
        }
        else if (earningPercent <= .3f)
        {
            return "laugh";
        }
        else if (earningPercent <= .5f)
        {
            return "laugh2";
        }
        else if (earningPercent <= .7f)
        {
            return "worm";
        }
        else if (earningPercent <= .9f)
        {
            return "half_eaten";
        }
        else if (earningPercent >= .9f)
        {
            return "sunglasses";
        }

        return "sliced";
    }
}
