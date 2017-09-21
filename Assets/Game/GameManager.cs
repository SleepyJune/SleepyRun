using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public LevelInfo level;
    
    [NonSerialized]
    public FloorManager floorManager;
    [NonSerialized]
    public MonsterManager monsterManager;
    [NonSerialized]
    public TouchInputManager touchInputManager;
    [NonSerialized]
    public ComboManager comboManager;

    public Player player;

    private int entityId = 0;
    private bool gameOver = false;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        floorManager = GetComponent<FloorManager>();
        monsterManager = GetComponent<MonsterManager>();
        touchInputManager = GetComponent<TouchInputManager>();
        comboManager = GetComponent<ComboManager>();
    }

    void Update()
    {
        DelayAction.OnUpdate();
    }

    public int GenerateEntityId()
    {
        return entityId++;
    }

    public Vector3 GetTouchPosition(Vector2 position, float height = 999)
    {
        if (player)
        {
            if(height == 999)
            {
                height = player.transform.position.y;
            }

            Ray ray = Camera.main.ScreenPointToRay(position);
            var groundPlane = new Plane(Vector3.up, new Vector3(0, height, player.transform.position.z));
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                return ray.GetPoint(rayDistance);
            }
        }

        return Vector3.zero;
    }    
}
