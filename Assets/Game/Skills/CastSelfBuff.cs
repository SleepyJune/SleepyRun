using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class CastSelfBuff : Skill
{
    public BuffObject buffObject;

    protected override void Cast(Vector3 startPos, Vector3 endPos, Unit target = null)
    {        
        owner.SelfBuff(buffObject);

        EndCast();
    }
}
