using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/GameoverOnCollectCount event")]
class GameOverOnCollectCount : StageEvent
{
    public override string eventName { get { return "Victory Condition"; } }

    //public Monster monster;

    public int collectCount = 1;

    public bool victory = true;

    public override void ExecuteEvent()
    {
        if (GameManager.instance.monsterManager.GetMonsterCollectCount(monster) >= collectCount)
        {
            GameManager.instance.AdvanceToNextWave(victory);
            isExecuted = true;
        }
    }
}
