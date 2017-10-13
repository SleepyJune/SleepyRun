using UnityEngine;

[CreateAssetMenu(menuName = "MonsterActions/Action Info")]
public class MonsterActionInfo : ScriptableObject
{
    public MonsterConditionCollection[] conditionCollections = new MonsterConditionCollection[0];
}
