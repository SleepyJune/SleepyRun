using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class HitInfo
{
    public Unit source;
    public Unit target;

    public Vector3 hitStart;
    public Vector3 hitEnd;

    public Vector3 force;

    public int damage;

    public GameObject hitParticle;

    public BuffObject buffOnHit;

    public int knockBackForce;
}
