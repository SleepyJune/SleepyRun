using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class CastSelfBuff : Skill
{
    public BuffObject buffObject;

    public override void Cast(Vector3 startPos, Vector3 endPos, Unit target = null)
    {
        var buff = buffObject.Initialize(owner);
        buff.ActivateBuff(owner);

        EndCast();
    }
}
