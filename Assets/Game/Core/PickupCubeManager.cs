using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Random = UnityEngine.Random;

public class PickupCubeManager : MonoBehaviour
{
    public PickupCube pickupCubePrefab;

    public SkillDatabase skillDatabase;

    [NonSerialized]
    public float spawnRate = 1f;

    [NonSerialized]
    public Transform pickupHolder;

    [NonSerialized]
    public Transform newSkillsHolder;

    Vector3[] lanes =
    {
        new Vector3(1.5f,0,0),
        new Vector3(0,0,0),
        new Vector3(-1.5f,0,0),
    };

    Player player;

    void Start()
    {
        pickupHolder = (new GameObject("Pickup Holder")).transform;
        newSkillsHolder = (new GameObject("Skills Holder")).transform;

        player = GameManager.instance.player;
    }

    /*void Update()
    {
        var random = Random.Range(0, spawnFrequency / Time.deltaTime);

        if (random <= 1 && !GameManager.instance.isGamePaused)
        {
            SpawnPickup();
        }
    }*/

    public void SpawnPickup()
    {
        var spawnDistance = 60;
        var randomDist = spawnDistance + Random.Range(-5, 5);
        var lane = lanes[Random.Range(0, 3)];
        var pos = lane + new Vector3(0, 1f, player.transform.position.z + randomDist);

        var pickup = Instantiate(pickupCubePrefab, pos, Quaternion.identity);
        pickup.transform.SetParent(pickupHolder);

        pickup.speed = Random.Range(5, 10);
    }

    public void SpawnPickupOnMonsterDeath(Monster monster)
    {
        var randomNum = Random.Range(0, 10);

        if (randomNum <= 1)
        {
            var pos = monster.transform.position + new Vector3(0, .5f, 0);
            var pickup = Instantiate(pickupCubePrefab, pos, Quaternion.identity);
            pickup.transform.SetParent(pickupHolder);

            pickup.speed = monster.speed;
        }
    }

    public void TryPickup()
    {
        var random = Random.Range(0, 1/spawnRate);
        
        if (random <= 1)
        {
            ActivatePickupCube();
        }
    }

    public void ActivatePickupCube()
    {
        var player = GameManager.instance.player;
        var skill = GenerateRandomSkill(player);

        player.SetNewSkill(skill);

        //GameManager.instance.scoreManager.AddScore(20);
    }

    public Skill GenerateRandomSkill(Unit owner)
    {
        int numSkills = skillDatabase.allSkills.Length;

        Skill skill = null;

        while(skill == null)
        {
            skill = skillDatabase.allSkills[Random.Range(0, numSkills)];
        }        

        Skill newSkill = Instantiate(skill, newSkillsHolder); //create a new skill from prefab
        newSkill.Initialize(owner);

        return newSkill;
    }
}
