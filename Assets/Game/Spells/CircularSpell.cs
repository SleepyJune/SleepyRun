using UnityEngine;

public class CircularSpell : Spell
{
    void Start()
    {
        DamageMonsters(transform.position);
    }

    void DamageMonsters(Vector3 hitPos)
    {
        var unitObjects = Physics.OverlapSphere(hitPos, radius, collisionMask);
        foreach (var unitObject in unitObjects)
        {
            var unit = unitObject.GetComponent<Unit>();
            if (unit != null && unit.canTakeDamage)
            {
                var unitPos = unit.transform.position;

                var dir = (unitPos - hitPos).normalized;
                dir.y = .15f;

                var force = dir * 1000;

                unit.TakeDamage(InitializeHitInfo(unit, hitPos, unitPos, force));
            }
        }
    }

    public override void Death()
    {
        isDead = true;
        Destroy(transform.gameObject);
    }
}
