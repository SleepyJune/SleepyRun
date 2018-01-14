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

    public bool explodeOnShatter = false;

    public float explosionForce = 1.0f;
    public Vector3 explosionPosition;

    Monster monster;

    void Awake()
    {
        monster = GetComponent<Monster>();        
    }

    public void MakeShattered(HitInfo hitInfo)
    {
        if (shatterPrefab == null)
        {
            return;
        }

        var shattered = Instantiate(shatterPrefab, transform.position, transform.rotation, 
            GameManager.instance.monsterManager.monsterHolder);

        shattered.transform.localScale = transform.localScale;
                
        foreach (var rb in shattered.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddForce(hitInfo.force);

            if (explodeOnShatter)
            {
                rb.AddExplosionForce(explosionForce, transform.position + explosionPosition, 2.0f);
            }

        }        

        Destroy(shattered, 2);
        Destroy(monster.gameObject);
    }
}
