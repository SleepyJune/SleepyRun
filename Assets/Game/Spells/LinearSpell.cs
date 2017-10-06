using UnityEngine;

public class LinearSpell : Spell
{
    [System.NonSerialized]
    public Vector3 start;
    [System.NonSerialized]
    public Vector3 end;

    public int maxDistance = 500;

    public GameObject particleOnHit;

    public bool wallCollision = true;

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
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

        var monster = collision.GetMonster();
        if (monster != null)
        {
            var dir = (transform.position - start);
            dir.y = .15f;

            var force = dir * 100;

            monster.TakeDamage(new HitInfo
            {
                hitStart = start,
                hitEnd = transform.position,
                force = force,
                damage = damage
            });

            if (particleOnHit)
            {
                //Instantiate(particleOnHit, monster.anim.transform);
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
