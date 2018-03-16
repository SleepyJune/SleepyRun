using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Lane
{
    left,
    mid,
    right
}

public enum GameMode
{
    All,
    Tutorial,
    Easy,
    Normal,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameMode gameMode = GameMode.Normal;
       
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
    [NonSerialized]
    public PickupCubeManager spawnPickupManager;
    [NonSerialized]
    public StageEventManager stageEventManager;
    [NonSerialized]
    public ScoreManager scoreManager;
    [NonSerialized]
    public GameTimerManager timerManager;
    [NonSerialized]
    public GameTextOverlayManager textOverlayManager;
    [NonSerialized]
    public LevelSelectManager levelSelectManager;
    [NonSerialized]
    public AdManager adManager;
        
    public delegate void Callback();
    public event Callback onUpdate;
    public event Callback onFixedUpdate;

    public Player player;

    private int entityId = 0;
    public bool isGameOver = false;

    public Transform canvas;

    public AudioSource audioSource;
            
    
    private ReviveWindow reviveWindow;

    public bool isBossFight = false;

    public float gameStartTime;
    
    public bool isGamePaused = false;
    public bool isMovingToNextWave = false;

    public float screenDPI;
    
    public float pauseCountdownEndTime = 0;

    public int reviveCount = 2;

    [NonSerialized]
    public int revivesUsed = 0;

    public int skipLevelGold = 10;

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
        spawnPickupManager = GetComponent<PickupCubeManager>();
        stageEventManager = GetComponent<StageEventManager>();
        scoreManager = GetComponent<ScoreManager>();
        timerManager = GetComponent<GameTimerManager>();
        textOverlayManager = GetComponent<GameTextOverlayManager>();
        levelSelectManager = GetComponent<LevelSelectManager>();
        adManager = GetComponent<AdManager>();

        gameStartTime = Time.time;

        screenDPI = Screen.dpi;      
    }

    void Start()
    {
        //victoryText = canvas.Find("VictoryText").gameObject;
        //waveText = canvas.Find("WaveText").gameObject;

        reviveWindow = canvas.Find("ReviveWindow").GetComponent<ReviveWindow>();
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

    public void AdvanceToNextWave(bool waveComplete = true)
    {
        if (waveComplete)
        {
            if (stageEventManager.AdvanceToNextWave())
            {
                GameOver(true);
            }
            else
            {
                MoveToNextArea();
            }
        }
        else
        {
            GameOver();
        }
    }

    public void MoveToNextArea()
    {
        isGamePaused = true;
        isMovingToNextWave = true;

        textOverlayManager.CreateWaveText();

        //moving...

        stageEventManager.GameResetStage();

        monsterManager.RemoveMonsters(true);

        DelayAction.Add(()=>stageEventManager.ShowRewardOverlay(),.5f);

        DelayAction.Add(EndMoveToNextArea, 5);
    }

    public void EndMoveToNextArea()
    {
        isGamePaused = false;
        isMovingToNextWave = false;
    }

    public void SkipLevels()
    {
        if (MoneyManager.instance.DecreaseGold(skipLevelGold))
        {
            Debug.Log("skip");
            levelSelectManager.LoadLevel(9, false);
        }
    }

    public void ShowReviveWindow()
    {
        if (reviveCount > 0 && adManager.isAdReady())
        {
            PauseGame();
            reviveWindow.SetReviveCount(reviveCount);
            reviveWindow.ShowWindow();
        }
        else
        {
            GameOverComplete();
        }
    }

    public void ReviveAccept()
    {
        PauseGame();
        adManager.ShowAdForRevive();
    }

    public void ReviveAdComplete()
    {
        isGameOver = false;
        weaponManager.DisableWeapon(false);
        player.Revive();

        reviveCount -= 1;
        revivesUsed += 1;

        scoreManager.stageRevivesUsed += 1;

        reviveWindow.HideWindow();
        ResumeGameText();
    }

    public void GameOverComplete()
    {        
        scoreManager.SetNewLevelStats(false);
        SceneChanger.ChangeScene("LevelComplete2");
    }

    public void ResumeGameText()
    {
        isGamePaused = false;
        textOverlayManager.CreateResumeTapText();
    }

    public void GameOverOnFired()
    {
        if (!isGameOver)
        {
            weaponManager.DisableWeapon(true);

            isGameOver = true;
            
            //textOverlayManager.CreateGameOverText();

            player.SetGameOverAnimation();

            //stageEventManager.tutorialController.SetFiredAnimation();

            textOverlayManager.CreateGameOverOnAppleMissed();

            DelayAction.Add(() => GameOverComplete(), 5);
        }
    }

    public void GameOver(bool levelComplete = false, bool reviveUsable = true)
    {
        if (!isGameOver)
        {
            weaponManager.DisableWeapon(true);

            isGameOver = true;

            if (!levelComplete)
            {
                //gameOverText.SetActive(true);

                textOverlayManager.CreateGameOverText();

                player.SetGameOverAnimation();

                if (reviveUsable)
                {
                    DelayAction.Add(() => ShowReviveWindow(), 5);
                }
                else
                {
                    DelayAction.Add(() => GameOverComplete(), 5);
                }
            }
            else
            {
                player.Victory();

                textOverlayManager.CreateVictoryText();
                                
                scoreManager.SetNewLevelStats(levelComplete);
                DelayAction.Add(() => SceneChanger.ChangeScene("GameComplete"), 5);
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

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }
}
