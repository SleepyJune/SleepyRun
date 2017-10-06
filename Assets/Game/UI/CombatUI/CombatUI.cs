using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class CombatUI : MonoBehaviour
{
    protected Weapon weapon;

    public abstract void Initialize(Weapon weapon);
    public abstract void End();
}
