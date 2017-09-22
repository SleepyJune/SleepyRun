using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterShatter : MonoBehaviour
{
    public GameObject target;

    Monster monster;

    void Awake()
    {
        monster = GetComponent<Monster>();        
    }

    public void MakeShattered(Vector3 force)
    {
        var shattered = Instantiate(target, transform.position, transform.rotation);
        
        foreach(var rb in shattered.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(force);
        }        

        Destroy(shattered, 2);
        Destroy(gameObject);
    }
}
