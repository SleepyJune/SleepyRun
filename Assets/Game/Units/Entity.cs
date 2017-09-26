using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [NonSerialized]
    public int id;

    public float speed = 1;

    [NonSerialized]
    public bool isDead = false;
}
