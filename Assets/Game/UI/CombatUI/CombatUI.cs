using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class CombatUI : MonoBehaviour
{
    protected Weapon weapon;

    public virtual void OnTouchStart(Touch touch) { }
    public virtual void OnTouchMove(Touch touch) { }
    public virtual void OnTouchEnd(Touch touch) { }
    public virtual void OnUpdate() { }
    public virtual void End() { }

    public abstract void Initialize(Weapon weapon);
}
