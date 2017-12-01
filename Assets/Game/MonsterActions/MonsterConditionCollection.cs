using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "MonsterActions/MonsterConditionCollection")]
public class MonsterConditionCollection : ScriptableObject
{
    public string description;

    public MonsterCondition[] conditions = new MonsterCondition[0];
    public MonsterActionCollection actionCollection;

    Monster monster;

    public void Initialize(Monster monster)
    {
        this.monster = monster;
        foreach(var condition in conditions)
        {
            condition.monster = monster;
            condition.conditionCollection = this;

            condition.Initialize();
        }
    }

    public bool CheckAndReact()
    {
        for(int i = 0; i < conditions.Length; i++)
        {
            if (!conditions[i].isSatisfied)
            {
                return false;
            }
        }

        if (actionCollection)
        {
            actionCollection.ExecuteAll();
        }

        return true;
    }
}
