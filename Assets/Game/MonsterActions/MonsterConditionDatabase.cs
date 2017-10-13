using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "MonsterActions/Condition Database")]
public class MonsterConditionDatabase : ScriptableObject
{
    public MonsterCondition[] allConditions = new MonsterCondition[0];
    
    public string[] allConditionOptions = new string[0];

    public MonsterAction[] allActions = new MonsterAction[0];

    public string[] allActionOptions = new string[0];
}
