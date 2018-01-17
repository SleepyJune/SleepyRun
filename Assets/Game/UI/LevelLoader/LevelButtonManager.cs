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

    void Start()
    {
        foreach(var stage in stageDatabase.databaseArray)
        {
            var newLevelButton = Instantiate(levelButtonPrefab);

            newLevelButton.transform.SetParent(levelButtonHolder, false);
            newLevelButton.SetActive(stage.stageId, true, 3, false, levelSelectManager);
        }

        scrollSnapObject.enabled = true;
    }    

}
