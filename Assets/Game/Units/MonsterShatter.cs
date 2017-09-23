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

    public void MakeShattered(HitInfo hitInfo)
    {
        var dir = (hitInfo.hitEnd - hitInfo.hitStart);
        dir.y = .15f;

        var force = dir * 50;

        var shattered = Instantiate(target, transform.position, transform.rotation);               
        
        //var hitParticle = Instantiate(hitInfo.hitParticle, pieceTrans);

        /*var hitParticle = Instantiate(
            hitInfo.hitParticle,
            transform.position + new Vector3(0, .5f, 0),
            Quaternion.LookRotation(-dir));

        hitParticle.transform.SetParent(pieceTrans);*/

        foreach (var rb in shattered.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(force);
        }        

        Destroy(shattered, 2);
        Destroy(gameObject);
    }
}
