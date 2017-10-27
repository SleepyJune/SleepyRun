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
    public Text timeText;
    public Text killCountText;

    void Start()
    {
        stats = SceneChanger.levelStats;

        if(stats == null)
        {
            SceneChanger.ChangeScene("IntroScreen2");
            return;
        }

        pointText.text = stats.points.ToString();
        killCountText.text = stats.monstersKilled.ToString();

        string minSec = string.Format("{0}:{1:00}", (int)stats.time / 60, (int)stats.time % 60);
        timeText.text = minSec;
    }
}
