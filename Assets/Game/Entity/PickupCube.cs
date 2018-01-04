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

    void OnTriggerEnter(Collider collision)
    {
        if (isDead) return;

        if (collision.gameObject.layer == LayerConstants.playerLayer)
        {
            Activate();
        }
    }

    public void Activate()
    {
        //GameManager.instance.damageTextManager.CreateDamageText(this, "+10", DamageTextType.Recovery);

        GameManager.instance.spawnPickupManager.ActivatePickupCube();

        Destroy(gameObject);
    }
}
