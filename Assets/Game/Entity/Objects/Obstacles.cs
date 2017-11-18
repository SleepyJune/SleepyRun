using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Obstacles : Traps
{
    void OnTriggerEnter(Collider collision)
    {
        if (isDead) return;

        if (collision.gameObject.layer == LayerConstants.monsterLayer)
        {
            var monster = collision.GetComponent<Monster>();
            if (monster && !monster.isDead)
            {
                monster.CollideWithObstacle(this);
            }
        }
    }
}
