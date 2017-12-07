using UnityEngine;

public class ColliderSpell : Spell
{
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
            var dir = (monster.transform.position - transform.position);
            dir.y = .15f;

            var force = dir * 100;

            monster.TakeDamage(InitializeHitInfo(monster, transform.position, transform.position, force));

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
