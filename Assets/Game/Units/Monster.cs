using UnityEngine;

public class Monster : Unit
{
    public void RemoveFromStage()
    {
        if (!isDead)
        {
            GameManager.instance.comboManager.BreakCombo();
        }
    }

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
