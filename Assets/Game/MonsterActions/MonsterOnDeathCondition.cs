using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterOnDeathCondition : MonsterCondition
{
    public int conditionRepeats = 0;

    public override void Initialize()
    {
        monster.OnDeath += SetTrue;
    }

    public override void CheckCondition()
    {

    }

    public void SetTrue(HitInfo hitInfo)
    {
        isSatisfied = true;


        //Debug.Log("OnDeath");

        var isExecuted = conditionCollection.CheckAndReact();
        if (isExecuted)
        {
            isSatisfied = false;
        }

        if(conditionRepeats == 0)
        {
            isSatisfied = false;
            monster.OnDeath -= SetTrue;
        }
        else
        {
            conditionRepeats -= 1;
        }
    }

    public override void Destroy()
    {

    }
}
