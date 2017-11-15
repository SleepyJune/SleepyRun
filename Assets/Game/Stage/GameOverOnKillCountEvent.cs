using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/GameoverOnKillCount event")]
class GameOverOnKillCountEvent : StageEvent
{
    public Monster monster;

    public int killCount = 1;

    public bool victory = true;

    public override void ExecuteEvent()
    {
        if (GameManager.instance.monsterManager.GetMonsterKillCount(monster) >= killCount)
        {
            GameManager.instance.AdvanceToNextWave(victory);
            isExecuted = true;
        }
    }
}
