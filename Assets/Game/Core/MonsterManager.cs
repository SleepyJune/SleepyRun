using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public Dictionary<int, Monster> monsters = new Dictionary<int, Monster>();
    
    Player player;
    LevelInfo level;

    FloorManager floorManager;

    Vector3[] lanes =
    {
        new Vector3(1.5f,0,0),
        new Vector3(0,0,0),
        new Vector3(-1.5f,0,0),
    };

    Transform monsterHolder;

    void Start()
    {
        level = GameManager.instance.level;
        player = GameManager.instance.player;
        floorManager = GameManager.instance.floorManager;

        monsterHolder = (new GameObject("Monster Holder")).transform;
    }

    void Update()
    {
        CheckMonsters();
    }

    void MakeMonster(Monster prefab)
    {
        var lastFloor = floorManager.lastFloor;
        if (lastFloor)
        {
            Vector3 lane = lanes[Random.Range(0, 3)];
            Vector3 spawnPos = player.transform.position + lane + new Vector3(0,0,20);

            var newMonster = Instantiate(prefab, spawnPos, Quaternion.Euler(new Vector3(0,180,0)));
            newMonster.transform.SetParent(monsterHolder);
            newMonster.id = GameManager.instance.GenerateEntityId();

            monsters.Add(newMonster.id, newMonster);
        }
    }

    void CheckMonsters()
    {
        foreach (var monsterInfo in level.monsters)
        {
            var random = Random.Range(0, monsterInfo.spawnFrequency / Time.deltaTime);

            if (random <= 1)
            {
                MakeMonster(monsterInfo.monster);
            }
        }

        List<Monster> removeList = new List<Monster>();
        foreach(var monster in monsters.Values)
        {
            if (player.transform.position.z - monster.transform.position.z > 5)
            {
                removeList.Add(monster);
                Destroy(monster.gameObject);
            }
        }

        foreach(var monster in removeList)
        {
            monsters.Remove(monster.id);
        }

    }
}