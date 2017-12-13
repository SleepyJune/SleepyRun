using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PointObjectUI : CombatUI
{
    public override void OnTouchStart(Touch touch)
    {
        var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);
        pos = GetLanePostion(pos);

        skill.CastSpell(pos, pos);

        //var newSpell = Instantiate(trap, pos, Quaternion.identity);        
    }

    public Vector3 GetLanePostion(Vector3 pos)
    {
        if(pos.x < -0.75f)
        {
            pos.x = -1.5f;
        }
        else if(pos.x > 0.75f)
        {
            pos.x = 1.5f;
        }
        else
        {
            pos.x = 0f;
        }

        pos.y = 0.5f;

        return pos;
    }
}
