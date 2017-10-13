using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "MonsterActions/Random Frequency")]
public class MonsterRandomCondition : MonsterCondition
{
    public float frequency = 1;

    public override void Initialize()
    {
        monster.OnMonsterUpdate += CheckCondition;
    }

    public override void CheckCondition()
    {
        isSatisfied = Random.Range(0, frequency / Time.deltaTime) <= 1;
    }

    public override void Destroy()
    {
        monster.OnMonsterUpdate -= CheckCondition;
    }
}
