using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/Random Monster Spawn")]
public class RandomIntervalSpawnMonsterEvent : StageEvent
{
    public override string eventName { get { return "Random Spawn"; } }

    //public Monster monster;
    public float zPositionStart = 0;
    public float zPositionEnd = 99999;

    public int zSpawnDistance = 60;

    public int maxOnScreen = 999;

    public float spawnFrequency = 1;

    public override void ExecuteEvent()
    {
        var playerZPos = GameManager.instance.player.transform.position.z;
        if (playerZPos >= zPositionStart && playerZPos <= zPositionEnd)
        {
            var random = Random.Range(0, spawnFrequency / Time.deltaTime);

            if (random <= 1)
            {
                int monsterCount;
                if (GameManager.instance.monsterManager.monsterCount.TryGetValue(monster.name, out monsterCount))
                {
                    if (monsterCount >= maxOnScreen)
                    {
                        return;
                    }
                }

                GameManager.instance.monsterManager.MakeMonster(monster, zSpawnDistance);
            }
        }
        else
        {
            if(playerZPos > zPositionEnd)
            {
                isExecuted = true;
            }
        }
    }
}
