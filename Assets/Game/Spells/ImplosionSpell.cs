using UnityEngine;

public class ImplosionSpell : Spell
{
    public GameObject particleOnHit;

    void Start()
    {
        DestroyMonsters(transform.position);
    }

    void DestroyMonsters(Vector3 hitPos)
    {
        var monstersMask = LayerMask.GetMask("Monsters");

        var monsterObjects = Physics.OverlapSphere(hitPos, radius, monstersMask);
        foreach (var monsterObject in monsterObjects)
        {
            var monster = monsterObject.GetComponent<Monster>();
            if (monster)
            {
                var monsterPos = monster.transform.position;

                var dir = (monsterPos - hitPos).normalized;
                //var dir = Vector3.Cross(hitPos, monsterPos).normalized;
                //dir = transform.rotation * dir;

                dir.y = .15f;

                var force = -dir * 2000;

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
    }

    public override void Death()
    {
        isDead = true;
        Destroy(transform.gameObject);
    }
}
