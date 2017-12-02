using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterSelfBuff : MonsterAction
{
    public BuffObject buff;

    public override bool Execute()
    {
        var newBuff = buff.Initialize();
        monster.ApplyBuff(newBuff);
        return true;
    }
}
