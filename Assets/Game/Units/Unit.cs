using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int id;

    public Animator anim;

    public float speed = 1;

    public bool isDead = false;
}
