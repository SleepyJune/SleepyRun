using System.Collections;
using UnityEngine;

public abstract class Spell : Entity
{
    [System.NonSerialized]
    public new Rigidbody rigidbody;
    [System.NonSerialized]
    public new Collider collider;

    [System.NonSerialized]
    public Unit source;

    public int damage;

    public float radius = 1;

    public LayerMask collisionMask;

    public GameObject particleOnHit;
    public BuffObject buffOnHit;

    public int knockBackForce = 0;

    [System.NonSerialized]
    public float startTime;

    protected virtual void Initialize()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        
        if(collider == null)
        {
            collider = GetComponentInChildren<Collider>();
        }
        
        if (source && source.gameObject.layer == LayerConstants.monsterLayer)
        {
            gameObject.layer = LayerConstants.monsterSpellLayer;
        }
        else
        {
            gameObject.layer = LayerConstants.playerSpellLayer;
        }

        startTime = Time.time;

        //var timeFlying = maxDistance/speed;        
    }

    public HitInfo InitializeHitInfo(Unit target, Vector3 hitStart, Vector3 hitEnd, Vector3 force)
    {
        HitInfo newHitInfo = new HitInfo
        {
            source = source,
            target = target,
            hitStart = hitStart,
            hitEnd = hitEnd,
            force = force,
            damage = damage,
            knockBackForce = knockBackForce,
            hitParticle = particleOnHit,
            buffOnHit = buffOnHit,
        };

        return newHitInfo;
    }

    public abstract void Death(); //spell destruction and clean up
}
