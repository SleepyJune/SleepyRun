using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public Queue<Floor> floors = new Queue<Floor>();

    [System.NonSerialized]
    public Floor currentFloor;

    [System.NonSerialized]
    public Floor lastFloor;

    public Floor floorPrefab;

    Player player;

    public Transform floorHolder;

    public float beltSpeed = 2.0f;

    float baseBeltSpeed = 2.0f;
    float slowPercent = 0;
    float slowEffectTime = 0;

    int numFloorsToPregenerate = 10;
    int numFloorsTillDestruction = 5;

    void Start()
    {
        player = GameManager.instance.player;

        //floorHolder = (new GameObject("Floor Holder")).transform;
    }

    void Update()
    {
        CheckFloors();

        floorHolder.position += Vector3.back * beltSpeed * Time.deltaTime;        
    }

    public void UpdateBeltSpeed()
    {
        beltSpeed = baseBeltSpeed * (1 - slowPercent);
    }

    public void SetBeltSpeed(float newSpeed)
    {
        baseBeltSpeed = newSpeed;
        UpdateBeltSpeed();
    }

    public void SetPercentSlow(float slow, float effectTime)
    {
        if (Time.time > slowEffectTime)
        {
            slowEffectTime = Time.time + effectTime;
            slowPercent = slow;
        }
        else
        {
            slowEffectTime += effectTime;
            slowPercent = slow;
        }

        DelayAction.Add(CheckSlow, effectTime);

        UpdateBeltSpeed();
    }

    public void CheckSlow()
    {
        if (Time.time > slowEffectTime)
        {
            slowPercent = 0.0f;
        }

        UpdateBeltSpeed();
    }

    void MakeFloor()
    {
        Vector3 spawnPos;

        if (lastFloor)
        {
            spawnPos = lastFloor.transform.position + new Vector3(0, 0, lastFloor.length);
        }
        else
        {
            spawnPos = player.transform.position;
            spawnPos.y = 0;
        }

        lastFloor = Instantiate(floorPrefab, spawnPos, Quaternion.identity);
        
        lastFloor.transform.SetParent(floorHolder);

        floors.Enqueue(lastFloor);
    }

    void CheckFloors()
    {
        if (floors.Count <= numFloorsToPregenerate)
        {
            MakeFloor();
        }

        var oldestFloor = floors.Peek();
        if (player.transform.position.z - oldestFloor.transform.position.z > oldestFloor.length * numFloorsTillDestruction)
        {
            floors.Dequeue();
            Destroy(oldestFloor.gameObject);
        }
    }
}