using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Entity
{
    public Animator anim;

    public int maxHealth = 10;
        
    [NonSerialized]
    public int health;

    public int defense = 0;

}
