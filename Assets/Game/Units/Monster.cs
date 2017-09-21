using UnityEngine;

public class Monster : Unit
{
    public void Death()
    {
        if (!isDead)
        {
            isDead = true;

            anim.SetTrigger("Die");
            anim.SetBool("isDead", true);

            /*if (deathAnimation)
            {
                DeleteUnit(deathAnimation.length);
            }
            else
            {
                DeleteUnit(0);
            }*/
        }
    }
}
