using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterShatterParticle : MonoBehaviour
{
    public GameObject particle;
    public Transform parent;   

    void Start()
    {
        Instantiate(particle, parent);
    }   
}
