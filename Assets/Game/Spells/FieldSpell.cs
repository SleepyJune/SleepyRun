using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldSpell : Spell
{
    Dictionary<Unit, float> affectedUnits = new Dictionary<Unit, float>();

    public bool isDOT = false; //damage overtime

    public float updateTime = .25f;

    float lastUpdateTime = 0;    
            
    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (isDOT)
        {
            if(Time.time - lastUpdateTime >= updateTime)
            {
                foreach(var pair in affectedUnits)
                {
                    ApplyDamage(pair.Key, pair.Value);
                }

                lastUpdateTime = Time.time;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (isDOT)
        {         

            var unit = collider.gameObject.GetComponent<Unit>();
            if (unit != null)
            {
                affectedUnits.Remove(unit);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        var unit = collider.gameObject.GetComponent<Unit>();
        if (unit != null && !unit.isDead)
        {
            if (!affectedUnits.ContainsKey(unit))
            {
                affectedUnits.Add(unit, Time.time);
                ApplyDamage(unit, 0);
            }
            else
            {
                ApplyDamage(unit, affectedUnits[unit]);
            }
        }            
    }

    void ApplyDamage(Unit unit, float previousTime)
    {
        if (isDead) return;
                
        //if (Time.time - previousTime < .25f) return;
        //affectedUnits[unit] = Time.time;

        if (gameObject.layer == LayerConstants.playerSpellLayer)
        {
            var monster = unit as Monster;
            if (monster != null)
            {
                var force = Vector3.zero;
                var monsterPos = monster.transform.position;

                monster.TakeDamage(InitializeHitInfo(monster, monsterPos, monsterPos, force));
            }
        }
        else
        {
            var player = unit as Player;
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

    public override void Death()
    {
        isDead = true;
        Destroy(transform.gameObject);
    }
}
