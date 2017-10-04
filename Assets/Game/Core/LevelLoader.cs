using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public StageInfoDatabase stageDatabase;

    public GameObject levelButton;

    public Transform stageList;

    void Start()
    {
        InitializeList();
    }

    void InitializeList()
    {
        foreach(var stageInfo in stageDatabase.databaseArray)
        {
            var newButton = Instantiate(levelButton, stageList);

            var text = newButton.transform.GetChild(0).gameObject;
            text.GetComponent<Text>().text = stageInfo.stageName;

            newButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(stageInfo));
        }
    }

    void LoadLevel(StageInfo stageInfo)
    {
        SceneChanger.currentStageInfo = stageInfo;
        SceneChanger.ChangeScene("GameScene");
        
    }
}
