using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/StageInfo")]
public class StageInfo : ScriptableObject
{
    public int stageId;

    public string stageName;

    public StageWave[] stageWaves = new StageWave[0];    
}
