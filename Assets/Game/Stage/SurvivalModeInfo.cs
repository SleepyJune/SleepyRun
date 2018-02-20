using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class SurvivalModeInfo : MonoBehaviour
{
    public float beltSpeed = 1.0f;

    public int appleToCollect = 5;

    public SurvivalModeMonsterInfo apple;
    public SurvivalModeMonsterInfo egg;
    public SurvivalModeMonsterInfo goldenApple;
    
    public int CalculateAppleToCollect()
    {
        float apples = apple.spawnFrequency == 0 ? 0 : 60.0f / apple.spawnFrequency;
        float eggs = egg.spawnFrequency == 0 ? 0 : 60.0f / egg.spawnFrequency;
        float goldenApples = goldenApple.spawnFrequency == 0 ? 0 : 60.0f / goldenApple.spawnFrequency;
        
        float sum = apples + eggs + 3 * goldenApples;
        
        float lowerLimit = sum * 0.667f;
        
        return (int)Math.Round(lowerLimit);
    }
}
