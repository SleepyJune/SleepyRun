using UnityEngine;

public class TargetedSpell : Spell
{
    public bool targetPlayer = true;

    void Start()
    {
        if (targetPlayer)
        {
            ApplyDamage(GameManager.instance.player);
        }

        Death();
    }

    void ApplyDamage(Unit unit)
    {
        if (isDead) return;
        
        if (unit != null && unit.canTakeDamage)
        {
            var force = Vector3.zero;
            var monsterPos = unit.transform.position;

            unit.TakeDamage(InitializeHitInfo(unit, monsterPos, monsterPos, force));
        }
    }

    public override void Death()
    {
        isDead = true;
        Destroy(transform.gameObject);
    }
}
