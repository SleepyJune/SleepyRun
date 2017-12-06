using UnityEngine;

public class CircularSpell : Spell
{
    //public GameObject particleOnHit;

    void Start()
    {
        DamageMonsters(transform.position);
    }

    void DamageMonsters(Vector3 hitPos)
    {
        var monsterObjects = Physics.OverlapSphere(hitPos, radius, collisionMask);
        foreach (var monsterObject in monsterObjects)
        {
            if (monsterObject.gameObject.layer == LayerConstants.monsterLayer)
            {
                var monster = monsterObject.GetComponent<Monster>();
                if (monster)
                {
                    var monsterPos = monster.transform.position;

                    var dir = (monsterPos - hitPos).normalized;
                    //var dir = Vector3.Cross(hitPos, monsterPos).normalized;
                    //dir = transform.rotation * dir;

                    dir.y = .15f;

                    var force = dir * 1000;

                    HitInfo hitInfo = new HitInfo
                    {
                        hitStart = hitPos,
                        hitEnd = monsterPos,
                        force = force,
                        damage = damage
                    };

                    monster.TakeDamage(hitInfo);
                }
            }
            else if (monsterObject.gameObject.layer == LayerConstants.playerLayer)
            {
                var player = monsterObject.GetComponent<Player>();
                if (player)
                {
                    player.TakeDamage(damage);
                }
            }
        }
    }

    public override void Death()
    {
        isDead = true;
        Destroy(transform.gameObject);
    }
}
