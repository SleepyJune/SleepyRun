using UnityEngine;

public class ColliderSpell : Spell
{
    public GameObject particleOnHit;

    void Awake()
    {
        Initialize();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (isDead) return;

        var monster = collision.GetMonster();
        if (monster != null)
        {
            monster.TakeDamage(damage);

            var dir = (monster.transform.position - transform.position);
            dir.y = .15f;

            var force = dir * 100;

            monster.Death(new HitInfo
            {
                hitStart = transform.position,
                hitEnd = transform.position,
                force = force,
            });

            if (particleOnHit)
            {
                Instantiate(particleOnHit, monster.anim.transform);
            }

            //isDead = true;
            //Destroy(transform.gameObject);
        }
    }

    public override void Death()
    {
        isDead = true;
        Destroy(transform.gameObject);
    }
}
