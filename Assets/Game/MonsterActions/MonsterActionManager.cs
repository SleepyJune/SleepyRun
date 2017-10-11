
using UnityEngine;

public class MonsterActionManager : MonoBehaviour
{
    public MonsterActionInfo actionInfo;

    [System.NonSerialized]
    public MonsterConditionCollection[] conditionCollections;

    float updateFrequency = .25f;
    float lastUpdateTime = 0;

    Monster monster;
        
    void Start()
    {
        monster = GetComponent<Monster>();

        conditionCollections = actionInfo.conditionCollections;

        foreach (var conditionCollection in conditionCollections)
        {
            conditionCollection.Initialize(monster);
            conditionCollection.actionCollection.Initialize(monster);
        }
    }

    void Update()
    {
        /*if(Time.time - lastUpdateTime >= updateFrequency)
        {
            CheckActions();
            lastUpdateTime = Time.time;
        }*/

        CheckCollections();
    }

    void CheckCollections()
    {
        foreach(var conditionCollection in conditionCollections)
        {
            conditionCollection.CheckAndReact();
        }
    }

    void Destroy()
    {
        foreach(var conditionCollection in conditionCollections)
        {
            foreach(var condition in conditionCollection.conditions)
            {
                condition.Destroy();
            }
        }
    }
}