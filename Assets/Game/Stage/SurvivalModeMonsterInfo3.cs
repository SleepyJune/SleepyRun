using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "SurvivalMode/MonsterInfo")]
public class SurvivalModeMonsterInfo2 : ScriptableObject
{
    public Monster monster;
        
    public AnimationCurve spawnRate = new AnimationCurve(new Keyframe(1, 1), new Keyframe(0, 0));
}
