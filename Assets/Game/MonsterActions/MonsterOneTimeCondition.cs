using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterOneTimeCondition : MonsterCondition
{
    public float delay = 0;

    public bool isRandom = false;
    public float frequency = 1f;
    
    public override void Initialize()
    {
        if (isRandom)
        {
            if(Random.Range(0, frequency) <= 1)
            {
                DelayAction.Add(SetTrue, delay);
            }
        }
        else
        {
            DelayAction.Add(SetTrue, delay);
        }

    }

    public override void CheckCondition()
    {
        
    }

    public void SetTrue()
    {
        isSatisfied = true;
        DelayAction.AddNextFrame(SetFalse);
    }

    public void SetFalse()
    {
        isSatisfied = false;
    }

    public override void Destroy()
    {
        
    }
}
