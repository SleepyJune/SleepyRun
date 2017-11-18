using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Serialization;

public class MonsterShatter : MonoBehaviour
{
    [FormerlySerializedAs("target")]
    public GameObject shatterPrefab;

    Monster monster;

    void Awake()
    {
        monster = GetComponent<Monster>();        
    }

    public void MakeShattered(HitInfo hitInfo)
    {
        if (shatterPrefab == null)
        {
            Destroy(gameObject);
            return;
        }

        var shattered = Instantiate(shatterPrefab, transform.position, transform.rotation, 
            GameManager.instance.monsterManager.monsterHolder);

        shattered.transform.localScale = transform.localScale;
                
        foreach (var rb in shattered.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(hitInfo.force);
        }        

        Destroy(shattered, 2);
        Destroy(gameObject);
    }
}
