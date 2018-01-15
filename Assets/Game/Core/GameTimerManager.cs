using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class GameTimerManager : MonoBehaviour
{
    public int timer = 60;

    public int levelTime = 0;

    public Text timerText;

    float updateInterval = 1.0f;

    float lastUpdateTime = 0;

    //public float gameStartTime = 0;
    public float gameTime = 0;

    [NonSerialized]
    public int totalGameTime = 0;

    void Start()
    {
        timerText.text = timer.ToString();
    }

    public void SetTime(int levelTime)
    {
        timer = levelTime;
        this.levelTime = levelTime;

        timerText.text = timer.ToString();
    }

    void UpdateTimer()
    {
        if (timer > 0)
        {
            timer -= 1;
            totalGameTime += 1;
            timerText.text = timer.ToString();
        }
    }

    void Update()
    {
        if(!GameManager.instance.isGamePaused && !GameManager.instance.isMovingToNextWave)
        {
            gameTime = Time.time;
        }

        if (gameTime - lastUpdateTime > updateInterval)
        {
            lastUpdateTime = gameTime;
            UpdateTimer();
        }
    }    
}
