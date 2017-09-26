using UnityEngine;

public class LinearSpell : Spell
{
    [System.NonSerialized]
    public Vector3 start;
    [System.NonSerialized]
    public Vector3 end;
    
    public int maxDistance = 500;

    public GameObject particleOnHit;

    public int wallLayer;

    void Awake()
    {
        Initialize();

        wallLayer = LayerMask.NameToLayer("Walls");
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
        var dir = (end - start).normalized;
        dir.y = 0;

        if (speed > 0)
        {
            rigidbody.AddForce(dir * speed);
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
                
        if(collision.gameObject.layer == wallLayer)
        {
            Death();
            return;
        }

        var monster = collision.gameObject.GetComponent<Monster>();        
        if (monster != null && !monster.isDead)
        {            
            monster.TakeDamage(damage);

            var dir = (transform.position - start);
            dir.y = .15f;

            var force = dir * 100;

            monster.Death(new HitInfo
            {
                hitStart = start,
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
