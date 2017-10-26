using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPickupManager : MonoBehaviour
{
    public PickupCube pickupCubePrefab;

    public Transform pickupHolder;

    void Start()
    {
        pickupHolder = (new GameObject("Pickup Holder")).transform;
    }

    public void SpawnPickup(Monster monster)
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
}
