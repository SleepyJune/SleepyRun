using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public static LevelSelectManager instance = null;

    public StageInfoDatabase stageDatabase;

    public Text missionText;

    string[] randomQuotes =
    {
        "Get to work!",
        "No slacking off!",
        "You deserve no breaks!",
        "You are lucky to even have a job!",
        "I want it finished ASAP!",
        "Get busy!",
        "Go Go Go!",
        "I expect good work from you!",
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnLevelButtonChanged(int level)
    {
        var stage = stageDatabase.databaseArray[level];

        string quote = randomQuotes[Random.Range(0, randomQuotes.Length)];

        missionText.text = stage.missionText + "\n" + quote;
    }

    public void LoadLevelEx(int stageID)
    {
        LoadLevel(stageID);
    }

    public void LoadLevel(int stageID, bool reload = true)
    {        
        if (stageID >= stageDatabase.databaseArray.Length)
        {
            return;   
        }

        StageInfo stageInfo = stageDatabase.databaseArray[stageID];

        if (stageInfo != null)
        {
            string lastLevelString = "LastLevelPlayed";
            PlayerPrefs.SetInt(lastLevelString, stageInfo.stageId);

            SceneChanger.currentStageInfo = stageInfo;
            SceneChanger.sceneToLoad = "GameScene";

            if (reload)
            {
                SceneChanger.ChangeScene("LoadingScene");
            }
            else
            {
                SceneChanger.ChangeScene("GameScene");
            }
        }
    }
}
