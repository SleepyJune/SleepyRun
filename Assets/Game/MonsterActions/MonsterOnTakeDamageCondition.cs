using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterOnTakeDamageCondition : MonsterCondition
{
    public int conditionRepeats = 0;

    public override void Initialize()
    {
        monster.OnTakeDamage += SetTrue;
    }

    public override void CheckCondition()
    {

    }

    public void SetTrue(HitInfo hitInfo, int finalDamage)
    {
        isSatisfied = true;

        var isExecuted = conditionCollection.CheckAndReact();
        if (isExecuted)
        {
            isSatisfied = false;
        }

        if (conditionRepeats == 0)
        {
            isSatisfied = false;
            monster.OnTakeDamage -= SetTrue;
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
