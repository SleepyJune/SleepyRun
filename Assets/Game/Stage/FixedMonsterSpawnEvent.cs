using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/Fixed Monster Spawn")]
class FixedMonsterSpawnEvent : StageEvent
{
    public Monster monster;
    public float zSpawnPosition = 100;

    public float spawnCount = 1;

    public override void ExecuteEvent()
    {
        var playerZPos = GameManager.instance.player.transform.position.z;
        if (playerZPos >= zSpawnPosition)
        {
            GameManager.instance.monsterManager.MakeMonster(monster);
            isExecuted = true;            
        }
    }
}
