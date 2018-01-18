using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/GameoverOnCoutdown event")]
public class GameOverOnCountdown : StageEvent
{
    public override string eventName { get { return "Victory Condition"; } }
    
    public int countdown = 60;

    public int collectCount = 30;

    public bool victoryOnCountdown = false;

    public bool victory = true;

    public override void ExecuteEvent()
    {
        if (GameManager.instance.timerManager.timer <= 0
            && (victoryOnCountdown || GameManager.instance.monsterManager.GetMonsterCollectCount(monster) >= collectCount))
        {
            GameManager.instance.AdvanceToNextWave(victory);
            isExecuted = true;
        }
    }
}
