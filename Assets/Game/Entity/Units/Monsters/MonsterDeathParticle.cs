using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterDeathParticle : MonoBehaviour
{
    public GameObject particle;

    public Vector3 offset;

    public void CreateParticle()
    {
        Instantiate(particle, transform.position + offset, transform.rotation);
    }
}
