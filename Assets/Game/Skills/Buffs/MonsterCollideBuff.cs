using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterCollideBuff : Buff
{
    private MonsterCollideBuffObject buffObj;

    public MonsterCollideBuff(Unit source, BuffObject buffObj, float duration) : base(source, buffObj, duration)
    {
        this.buffObj = buffObj as MonsterCollideBuffObject;
    }

    public override void ActivateBuff(Unit target)
    {
        this.unit = target;

        if(buffObj.effectPrefab != null)
        {
            var explosion = GameObject.Instantiate(buffObj.effectPrefab, source.transform.position, Quaternion.identity);
        }

        if(buffObj.functionType == MonsterCollideBuffObject.EffectFunctionType.Apple)
        {
            GameManager.instance.spawnPickupManager.TryPickup();
        }
        else if(buffObj.functionType == MonsterCollideBuffObject.EffectFunctionType.GoldenApple)
        {
            GameManager.instance.spawnPickupManager.SpawnUltimateSkill();
        }
        else
        {

        }
    }

    public override void EndBuff()
    {

    }
}
