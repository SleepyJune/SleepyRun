using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class SpawnerSpell : Spell
{
    public Spell[] spells;
    
    public Vector3[] startPositions;
    public Vector3[] directions;

    void Awake()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        for (int i = 0; i < startPositions.Length; i++)
        {
            var pos = GameManager.instance.player.transform.position + startPositions[i];

            var dir = directions.Length > 1 ? directions[i] : directions[0];
            var spell = spells.Length > 1 ? spells[i] : spells[0];

            var newObject = Instantiate(spell.gameObject, pos, Quaternion.Euler(dir));
            var newSpell = newObject.GetComponent<Spell>();
            newSpell.source = source;           
                     
        }

        Destroy(gameObject);
    }

    public override void Death()
    {

    }
}
