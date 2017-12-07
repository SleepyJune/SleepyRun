using UnityEngine;

public class CannonSpell : Spell
{
    [System.NonSerialized]
    public Vector3 start;
    [System.NonSerialized]
    public Vector3 end;

    public float angle = 45f;

    public float drag = 0;

    public bool targetCharacter = true;

    public int maxDistance = 500;

    public float initialCollisionTime = 0;
        
    void Start()
    {
        Initialize();
        SetVelocity();
    }

    void Update()
    {
        if (Vector3.Distance(start, transform.position) > maxDistance)
        {
            Destroy(transform.gameObject);
        }
    }

    public void SetVelocity()
    {
        if (start == Vector3.zero)
        {
            start = transform.position;
        }

        if (end == Vector3.zero)
        {
            if (targetCharacter)
            {
                end = GameManager.instance.player.transform.position;
            }
            else
            {
                end = transform.position + transform.forward * maxDistance;
            }            
        }

        var dir = (end - start).normalized;
        dir.y = 0;

        dir = Quaternion.AngleAxis(-angle, transform.right) * dir;

        var distance = (end - start).magnitude;

        var launchSpeed = (1 + drag) * ProjectileMath.LaunchSpeed(distance, 0, -Physics.gravity.y, angle);
                
        if (launchSpeed > 0)
        {
            rigidbody.AddForce(dir * launchSpeed, ForceMode.Impulse);
        }

        if (dir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(dir);
            rigidbody.MoveRotation(newRotation);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (isDead || Time.time - startTime <= initialCollisionTime) return;

        if (!(collisionMask == (collisionMask | (1 << collision.gameObject.layer)))) return; //if layer not in collisionMask

        if (collision.gameObject.layer == LayerConstants.wallLayer
            || collision.gameObject.layer == LayerConstants.groundLayer)
        {
            Death();
            return;
        }

        if (gameObject.layer == LayerConstants.playerSpellLayer)
        {
            var monster = collision.GetMonster();
            if (monster != null)
            {
                var dir = (transform.position - start);
                dir.y = .15f;

                var force = dir * 100;

                monster.TakeDamage(InitializeHitInfo(monster, start, transform.position, force));

                //isDead = true;
                //Destroy(transform.gameObject);
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

        if (particleOnHit)
        {
            Instantiate(particleOnHit, transform.position, Quaternion.identity);
        }

        Destroy(transform.gameObject);
    }
}
