using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterActionCollection : MonoBehaviour
{
    public MonsterAction[] actions = new MonsterAction[0];

    Monster monster;

    public void Initialize(Monster monster)
    {
        this.monster = monster;

        foreach(var action in actions)
        {
            action.monster = monster;
        }
    }

    public void ExecuteAll()
    {
        for(int i=0;i< actions.Length; i++)
        {
            actions[i].Execute();
        }
    }
}
