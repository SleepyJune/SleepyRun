using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public Dictionary<int, Monster> monsters = new Dictionary<int, Monster>();
    public Dictionary<string, int> monsterCount = new Dictionary<string, int>();
    public Dictionary<string, int> monsterKillCount = new Dictionary<string, int>();
    public Dictionary<string, int> monsterCollectedCount = new Dictionary<string, int>();
    public Dictionary<string, int> stageMonsterCollectedCount = new Dictionary<string, int>();
    public Dictionary<string, int> missedMonsterCount = new Dictionary<string, int>();

    public Dictionary<string, int> totalMonsterKillCount = new Dictionary<string, int>();

    public Dictionary<Monster, int> monsterSpawnCount = new Dictionary<Monster, int>();

    public GameObject moneyExplosionPrefab;

    Player player;

    FloorManager floorManager;
    
    Vector3[] lanes =
    {
        new Vector3(1.5f,0,0),
        new Vector3(0,0,0),
        new Vector3(-1.5f,0,0),
    };

    public Transform monsterHolder;
    public Transform immovableMonsterHolder;

    float spawnRadius = 1f;

    void Start()
    {
        player = GameManager.instance.player;
        floorManager = GameManager.instance.floorManager;

        //monsterHolder = (new GameObject("Monster Holder")).transform;

        GameManager.instance.stageEventManager.OnStageResetEvent += OnStageReset;
    }

    void Update()
    {
        RemoveDeadMonsters();
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

        newMonster.name = newMonster.name.Replace("(Clone)", "");

        if (newMonster.isImmovable)
        {            
            newMonster.transform.SetParent(immovableMonsterHolder);
        }
        else
        {
            newMonster.transform.SetParent(monsterHolder);
        }

        monsters.Add(newMonster.id, newMonster);

        AddMonsterCount(newMonster);

        return newMonster;
    }

    public void AddMonsterCollectedCount(Monster monster)
    {
        IncreaseDatabaseCount(monsterCollectedCount, monster);
        IncreaseDatabaseCount(stageMonsterCollectedCount, monster);
    }

    public void AddMonsterCount(Monster monster)
    {
        IncreaseDatabaseCount(monsterCount, monster);

        int count;
        if (monsterSpawnCount.TryGetValue(monster, out count))
        {
            monsterSpawnCount[monster] = count + 1;
        }
        else
        {
            monsterSpawnCount.Add(monster, 1);
        }        
    }
        
    public void AddMissedMonsterCount(Monster monster)
    {
        IncreaseDatabaseCount(missedMonsterCount, monster);
    }

    public void DecreaseMonsterCount(Monster monster)
    {
        int count;
        if (monsterCount.TryGetValue(monster.name, out count))
        {
            monsterCount[monster.name] = count - 1;
        }
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

        GameManager.instance.stageEventManager.UpdateVictoryCondition(monster, killCount + 1);

        IncreaseDatabaseCount(totalMonsterKillCount, monster);
    }

    public void OnStageReset()
    {
        monsterKillCount = new Dictionary<string, int>();
        stageMonsterCollectedCount = new Dictionary<string, int>();
    }

    public void IncreaseDatabaseCount(Dictionary<string, int> database, Monster monster)
    {
        int count;
        if (database.TryGetValue(monster.name, out count))
        {
            database[monster.name] = count + 1;
        }
        else
        {
            database.Add(monster.name, 1);
        }
    }

    public int GetDatabaseCount(Dictionary<string, int> database, Monster monster)
    {
        int count;
        if (database.TryGetValue(monster.name, out count))
        {
            return count;
        }
        else
        {
            return 0;
        }
    }

    public int GetMonsterKillCount(Monster monster)
    {
        return GetDatabaseCount(monsterKillCount, monster);
    }

    public int GetMonsterCollectCount(Monster monster)
    {
        return GetDatabaseCount(monsterCollectedCount, monster);
    }

    public int GetStageMonsterCollectCount(Monster monster)
    {
        return GetDatabaseCount(stageMonsterCollectedCount, monster);
    }

    public int GetMonsterSpawnCount(MonsterCollisionMask filterMonsterType)
    {
        return monsterSpawnCount.Where(p=>(p.Key.monsterType & filterMonsterType) != 0).Sum(p => p.Value);
    }

    public int GetKillCount()
    {
        return monsterKillCount.Values.Sum(count => count);
    }

    public int GetMissedCount()
    {
        return missedMonsterCount.Values.Sum(count => count);
    }

    public void SetDead(Monster monster)
    {
        monster.isDead = true;
        DecreaseMonsterCount(monster);
    }

    public void CreateMoneyExplosion(Vector3 pos)
    {
        var explosion = Instantiate(moneyExplosionPrefab, pos, Quaternion.identity);
    }

    public void RemoveMonster(Monster monster)
    {
        if (monster)
        {
            monsters.Remove(monster.id);
            Destroy(monster.gameObject);
        }        
    }

    public void RemoveDeadMonsters()
    {
        foreach (var pair in monsters.ToList())
        {
            if(pair.Value == null)
            {
                monsters.Remove(pair.Key);
            }
            else if(pair.Value.isDead)
            {
                RemoveMonster(pair.Value);
            }
        }
    }

    public void RemoveMonsters(bool forceRemove = false)
    {
        foreach (var monster in monsters.Values)
        {
            monster.isDead = true;
        }
    }
}