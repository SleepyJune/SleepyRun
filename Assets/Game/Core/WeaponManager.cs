using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon[] weapons;

    public Weapon defaultWeapon;

    public Weapon currentWeapon;

    bool usingUlt = false;

    void Start()
    {
        if(currentWeapon == null)
        {
            SwitchWeapons(defaultWeapon);
        }        
    }

    public void UseUltimate()
    {
        if (!GameManager.instance.player.isDead)
        {
            if (!usingUlt && currentWeapon.ultimateUI != null)
            {
                usingUlt = true;

                currentWeapon.combatUI.End();
                currentWeapon.ultimateUI.Initialize(currentWeapon);
            }
        }
    }

    public void EndUltimate()
    {
        if (usingUlt && currentWeapon.ultimateUI)
        {
            usingUlt = false;

            currentWeapon.ultimateUI.End();
            currentWeapon.combatUI.Initialize(currentWeapon);
        }
    }

    public void DisableWeapon()
    {
        if (currentWeapon)
        {
            if (usingUlt)
            {
                currentWeapon.ultimateUI.End();
                usingUlt = false;
            }
            else
            {
                currentWeapon.combatUI.End();
            }
        }
    }

    public void SwitchWeapons(Weapon newWeapon)
    {
        if (!GameManager.instance.player.isDead)
        {
            if (currentWeapon)
            {
                if (usingUlt)
                {
                    currentWeapon.ultimateUI.End();
                    usingUlt = false;
                }
                else
                {
                    currentWeapon.combatUI.End();
                }
            }

            currentWeapon = newWeapon;
            currentWeapon.combatUI.Initialize(newWeapon);
        }
    }
}
