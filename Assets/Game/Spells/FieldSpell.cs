using UnityEngine;

public class FieldSpell : Spell
{
    void Start()
    {
        Initialize();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (isDead) return;

        if (gameObject.layer == LayerConstants.playerSpellLayer)
        {
            var monster = collision.GetMonster();
            if (monster != null)
            {
                var force = Vector3.zero;
                var monsterPos = monster.transform.position;

                monster.TakeDamage(InitializeHitInfo(monster, monsterPos, monsterPos, force));
            }
        }
        else
        {
            var player = collision.GetPlayer();
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
