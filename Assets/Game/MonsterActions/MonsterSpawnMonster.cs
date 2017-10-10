using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "MonsterActions/Spawn Monster")]
public class MonsterSpawnMonster : MonsterAction
{
    public Monster spawnMonster;

    Vector3[] lanes =
    {
        new Vector3(1.5f,0,0),
        new Vector3(0,0,0),
        new Vector3(-1.5f,0,0),
    };

    public override bool Execute()
    {
        Vector3 lane = lanes[Random.Range(0, 3)];
        Vector3 spawnPos = monster.transform.position + lane;

        var spawnZPosition = (int)(monster.transform.position.z - GameManager.instance.player.transform.position.z);
        
        //var obj = Instantiate(spawnMonster, spawnPos, monster.transform.rotation);
        GameManager.instance.monsterManager.MakeMonster(spawnMonster, spawnZPosition);
        //var newMonster = obj.GetComponent<Monster>();

        return true;
    }
}
