using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterFixedTimingCondition : MonsterCondition
{
    public float frequency = 1;

    float nextActiveTime;

    public override void Initialize()
    {
        monster.OnMonsterUpdate += CheckCondition;
        nextActiveTime = Time.time + frequency;
    }

    public override void CheckCondition()
    {
        if(Time.time >= nextActiveTime)
        {
            isSatisfied = true;
            nextActiveTime = Time.time + frequency;
        }
        else
        {
            isSatisfied = false;
        }        
    }

    public override void Destroy()
    {
        monster.OnMonsterUpdate -= CheckCondition;
    }
}
