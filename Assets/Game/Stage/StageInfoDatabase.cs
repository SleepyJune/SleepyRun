using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/StageInfo Database")]
public class StageInfoDatabase : ScriptableObject
{
    public Dictionary<int, StageInfo> database = new Dictionary<int, StageInfo>();

    public StageInfo[] databaseArray;

    void Awake()
    {
        
    }

    void InitializeDatabase()
    {
        foreach(var stageInfo in databaseArray)
        {
            database.Add(stageInfo.stageId, stageInfo);
        }
    }
}
