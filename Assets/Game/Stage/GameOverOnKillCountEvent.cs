using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/GameoverOnKillCount event")]
class GameOverOnKillCountEvent : StageEvent
{
    public override string eventName { get { return "Victory Condition"; } }

    //public Monster monster;

    public int count = 1;

    public bool collectMonster = false;

    public bool victory = true;

    public override void ExecuteEvent()
    {
        if (collectMonster)
        {
            if (GameManager.instance.monsterManager.GetMonsterCollectCount(monster) >= count)
            {
                GameManager.instance.AdvanceToNextWave(victory);
                isExecuted = true;
            }
        }
        else
        {
            if (GameManager.instance.monsterManager.GetMonsterKillCount(monster) >= count)
            {
                GameManager.instance.AdvanceToNextWave(victory);
                isExecuted = true;
            }
        }
    }
}
