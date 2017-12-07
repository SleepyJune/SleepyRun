using UnityEngine;

public class LinearSpell : Spell
{
    [System.NonSerialized]
    public Vector3 start;
    [System.NonSerialized]
    public Vector3 end;
    
    public int maxDistance = 500;
        
    public bool wallCollision = true;
        
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
            end = transform.position + transform.forward * maxDistance;
        }
        
        var dir = (end - start).normalized;
        dir.y = 0;

        if (speed > 0)
        {
            rigidbody.AddForce(dir * speed, ForceMode.Impulse);
        }

        if (dir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(dir);
            rigidbody.MoveRotation(newRotation);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (isDead) return;

        if (wallCollision && collision.gameObject.layer == LayerConstants.wallLayer)
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
        Destroy(transform.gameObject);
    }
}
