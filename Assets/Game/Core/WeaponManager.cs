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
        if (currentWeapon.ultimateUI != null)
        {
            currentWeapon.combatUI.Destroy();
            currentWeapon.ultimateUI.Initialize(currentWeapon);
            usingUlt = true;
        }
    }

    public void EndUltimate()
    {
        if (currentWeapon.ultimateUI)
        {
            currentWeapon.ultimateUI.Destroy();
            currentWeapon.combatUI.Initialize(currentWeapon);
            usingUlt = false;
        }
    }

    public void SwitchWeapons(Weapon newWeapon)
    {
        if (currentWeapon)
        {
            currentWeapon.combatUI.Destroy();

            if (usingUlt)
            {
                currentWeapon.ultimateUI.Destroy();
                usingUlt = false;
            }
        }      
        
        currentWeapon = newWeapon;
        currentWeapon.combatUI.Initialize(newWeapon);
    }
}
