using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class LevelButtonManager : MonoBehaviour
{
    public GameObject levelSelectManager;
    public LevelButton levelButtonPrefab;

    public Transform levelButtonHolder;

    public StageInfoDatabase stageDatabase;

    public UnityEngine.UI.Extensions.HorizontalScrollSnap scrollSnapObject;

    int topPassedLevel = 0;
    int currentLevel = 0;

    void Start()
    {
        foreach(var stage in stageDatabase.databaseArray)
        {
            var newLevelButton = Instantiate(levelButtonPrefab);

            newLevelButton.transform.SetParent(levelButtonHolder, false);
            newLevelButton.SetActive(stage.stageId, true, 3, false, levelSelectManager);
        }
                
        scrollSnapObject.enabled = true;

        Invoke("GotoCurrentLevel", .05f);
    }    

    void GotoCurrentLevel()
    {
        /*string topLevelString = "TopLevelPassed";
        if (PlayerPrefs.HasKey(topLevelString))
        {
            topPassedLevel = PlayerPrefs.GetInt(topLevelString);
        }
        else
        {
            topPassedLevel = 1;
            //topPassedLevel = stageDatabase.databaseArray.Length - 2;
        }*/

        string lastLevelString = "LastLevelPlayed";
        if (PlayerPrefs.HasKey(lastLevelString))
        {
            currentLevel = Math.Max(0, PlayerPrefs.GetInt(lastLevelString) - 1);
        }
        else
        {
            currentLevel = 0;// topPassedLevel;
        }

        scrollSnapObject.GoToScreen(currentLevel);
    }

}
