using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class BossMonster : Monster
{
    public Vector3 startPosition;

    [NonSerialized]
    public Player player;

    void Start()
    {
        player = GameManager.instance.player;
        transform.position = player.transform.position + startPosition;

        transform.SetParent(player.transform);
    }
        
    public override void Death(HitInfo hitInfo)
    {
        if (!isDead)
        {
            isDead = true;

            GameManager.instance.monsterManager.AddKillCount(this);

            if (anim)
            {
                anim.SetTrigger("Die");
                anim.SetBool("isDead", true);
            }

            Destroy(gameObject);
        }
    }
}
