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

    public float GetRelativeSizeRatio()
    {
        var distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        return (15.5f * 55) / (distance * Camera.main.fieldOfView);
    }
}
