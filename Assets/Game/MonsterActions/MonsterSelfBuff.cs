using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "MonsterActions/Self Buff")]
public class MonsterSelfBuff : MonsterAction
{
    public BuffObject buff;

    public override bool Execute()
    {
        var newBuff = buff.InitializeBuff();
        monster.ApplyBuff(newBuff);
        return true;
    }
}
