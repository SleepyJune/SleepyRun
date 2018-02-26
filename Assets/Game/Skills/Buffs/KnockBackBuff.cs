using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class KnockBackBuff : Buff
{
    private KnockBackBuffObject buffObj;

    public KnockBackBuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as KnockBackBuffObject;
    }

    public override void ActivateBuff(Unit unit)
    {        
        var monster = unit as Monster;

        if (monster)
        {
            if (monster.monsterType == MonsterCollisionMask.Good)
            {
                if (unit.transform.position.x > 0.1)
                {
                    unit.transform.position -= new Vector3(buffObj.knockBackForce, 0, 0);
                }
                else if (unit.transform.position.x < -0.1)
                {
                    unit.transform.position += new Vector3(buffObj.knockBackForce, 0, 0);
                }
                else
                {
                    //unit.transform.position = new Vector3(buffObj.knockBackForce, 0, 0);
                }
            }
            else
            {
                unit.transform.position += new Vector3(0, 0, buffObj.knockBackForce);
            }
        }
    }

    public override void EndBuff()
    {

    }
}
