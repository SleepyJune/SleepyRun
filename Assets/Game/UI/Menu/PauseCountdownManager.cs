using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class PauseCountdownManager : MonoBehaviour
{
    public Text text;

    public int countdownTime;

    int time;
    int tickFrequency = 1;
    float lastTick;

    void Awake()
    {
        StartCoroutine(ResumeCountdown());
    }

    IEnumerator ResumeCountdown()
    {
        var endPauseTime = Time.realtimeSinceStartup + countdownTime;

        time = countdownTime;

        while (Time.realtimeSinceStartup < endPauseTime)
        {
            if(Time.realtimeSinceStartup - lastTick > tickFrequency)
            {
                text.text = time.ToString();
                time -= tickFrequency;
                lastTick = Time.realtimeSinceStartup;
            }

            yield return 0;
        }

        text.text = "GO";
    }
}