using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public Dictionary<int, Monster> monsters = new Dictionary<int, Monster>();
    public Dictionary<string, int> monsterCount = new Dictionary<string, int>();
    public Dictionary<string, int> monsterKillCount = new Dictionary<string, int>();

    Player player;
    LevelInfo level;

    FloorManager floorManager;
    
    Vector3[] lanes =
    {
        new Vector3(1.5f,0,0),
        new Vector3(0,0,0),
        new Vector3(-1.5f,0,0),
    };

    public Transform monsterHolder;

    float spawnRadius = 1f;

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

    public Monster MakeMonster(Monster prefab, int monsterSpawnDistance = 60)
    {
        var lastFloor = floorManager.lastFloor;
        if (lastFloor)
        {
            Vector3 lane = lanes[Random.Range(0, 3)];
            Vector3 spawnPos = lane + new Vector3(0,.5f, player.transform.position.z + monsterSpawnDistance);
                        
            int maxTries = 10;
            bool foundPlacement = true;
            for (int tries = 0; tries < maxTries; tries++) //prevent monsters spawning on top of each other
            {
                if (tries == maxTries-1)
                {
                    foundPlacement = false;
                    break;
                }

                var colliders = Physics.OverlapSphere(spawnPos, spawnRadius, LayerConstants.monsterMask);
                if (colliders.Length == 0)
                {
                    break;
                }
                else
                {
                    if(colliders.Length == 1)
                    {
                        var boss = colliders[0].GetComponent<BossMonster>();
                        if (boss)
                        {
                            break;
                        }
                    }

                    var randomDist = monsterSpawnDistance + Random.Range(-5, 5);
                    lane = lanes[Random.Range(0, 3)];
                    spawnPos = lane + new Vector3(0, .5f, player.transform.position.z + randomDist);
                }                
            }

            if (foundPlacement)
            {
                return CreateNewMonster(prefab, spawnPos, Quaternion.Euler(new Vector3(0, 180, 0)));                
            }
        }

        return null;
    }

    public Monster CreateNewMonster(Monster prefab, Vector3 spawnPos, Quaternion rotation)
    {
        var newMonster = Instantiate(prefab, spawnPos, rotation);
        newMonster.transform.SetParent(monsterHolder);

        monsters.Add(newMonster.id, newMonster);

        return newMonster;
    }

    public void AddKillCount(Monster monster)
    {
        int killCount;
        if(monsterKillCount.TryGetValue(monster.name, out killCount))
        {
            monsterKillCount[monster.name] = killCount + 1;
        }
        else
        {
            monsterKillCount.Add(monster.name, 1);
        }
    }

    public int GetMonsterKillCount(Monster monster)
    {
        int killCount;
        if (monsterKillCount.TryGetValue(monster.name, out killCount))
        {
            return killCount;
        }
        else
        {
            return 0;
        }
    }

    public int GetKillCount()
    {
        return monsterKillCount.Values.Sum(count => count);
    }
    
    void CheckMonsters()
    {
        /*foreach (var monsterInfo in level.monsters)
        {
            var random = Random.Range(0, monsterInfo.spawnFrequency / Time.deltaTime);

            if (random <= 1)
            {
                MakeMonster(monsterInfo.monster);
            }
        }*/

        List<int> removeList = new List<int>();
        foreach(var pair in monsters)
        {
            var monster = monsters[pair.Key];

            if (monster != null)
            {
                if (player.transform.position.z - monster.transform.position.z > 0)
                {
                    removeList.Add(monster.id);

                    monster.RemoveFromStage();

                    Destroy(monster.gameObject);
                }
            }
            else
            {
                removeList.Add(pair.Key);
            }
        }

        foreach(var key in removeList)
        {
            monsters.Remove(key);
        }

    }
}