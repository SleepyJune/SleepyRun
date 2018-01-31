using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Database/Monster Database")]
public class MonsterDatabase : ScriptableObject
{
    public Monster[] allMonsters = new Monster[0];
}
