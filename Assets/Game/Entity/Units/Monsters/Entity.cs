using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [NonSerialized]
    public int id;

    public string displayName;

    public int prefabID;

    public float speed = 0; //for spells
        
    [NonSerialized]
    public bool isDead = false;

    public float GetRelativeSizeRatio()
    {
        var distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        return (15.5f * 55) / (distance * Camera.main.fieldOfView);
    }

    public override bool Equals(object obj)
    {
        if (obj is Entity)
        {
            return Equals(this);
        }

        return false;
    }

    public bool Equals(Entity obj)
    {
        return obj.id == id;
    }

    public override int GetHashCode()
    {
        return id;
    }
}
