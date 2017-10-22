using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Lane
{
    left,
    mid,
    right
}

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
    [NonSerialized]
    public WeaponManager weaponManager;
    [NonSerialized]
    public DamageTextController damageTextManager;

    public delegate void Callback();
    public event Callback onUpdate;
    public event Callback onFixedUpdate;

    public Player player;

    private int entityId = 0;
    public bool isGameOver = false;

    public Transform canvas;

    private GameObject gameOverText;
    private GameObject victoryText;

    public bool isBossFight = false;
                
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
        weaponManager = GetComponent<WeaponManager>();
        damageTextManager = GetComponent<DamageTextController>();
    }

    void Start()
    {
        gameOverText = canvas.Find("GameOverText").gameObject;
        victoryText = canvas.Find("VictoryText").gameObject;
    }

    void Update()
    {
        DelayAction.OnUpdate();

        if (onUpdate != null)
        {
            onUpdate();
        }
    }

    void FixedUpdate()
    {        
        if (onFixedUpdate != null)
        {
            onFixedUpdate();
        }
    }

    public void SetBossFight(bool isBossFight)
    {
        this.isBossFight = isBossFight;
        player.isBossFight = isBossFight;
    }

    public int GenerateEntityId()
    {
        return entityId++;
    }

    public void GameOver(bool levelComplete = false)
    {
        if (!isGameOver)
        {
            weaponManager.DisableWeapon();

            isGameOver = true;

            if (!levelComplete)
            {
                gameOverText.SetActive(true);
                gameOverText.GetComponent<Animation>().Play("GameOverAnimation");

                DelayAction.Add(() => SceneChanger.ChangeScene("LevelLoader"), 5);
            }
            else
            {
                player.Victory();

                victoryText.SetActive(true);
                victoryText.GetComponent<Animation>().Play("GameOverAnimation");

                DelayAction.Add(() => SceneChanger.ChangeScene("LevelLoader"), 5);
            }
        }
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
