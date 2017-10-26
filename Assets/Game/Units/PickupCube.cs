using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class PickupCube : Entity
{
    Player player;

    void Start()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!isDead)
        {
            if (speed != 0)
            {
                transform.position -= Vector3.forward * speed * Time.deltaTime;
            }
        }

        if (player.transform.position.z - transform.position.z > 0)
        {
            Destroy(gameObject);
        }
    }

    public void Activate(Player player)
    {
        player.GainHealth(10);
        Destroy(gameObject);
    }
}
