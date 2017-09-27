using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon defaultWeapon;

    Weapon currentWeapon;

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
        }
    }

    public void EndUltimate()
    {
        if (currentWeapon.ultimateUI)
        {
            currentWeapon.ultimateUI.Destroy();
            currentWeapon.combatUI.Initialize(currentWeapon);
        }
    }

    public void SwitchWeapons(Weapon newWeapon)
    {
        if (currentWeapon)
        {
            currentWeapon.combatUI.Destroy();
        }

        currentWeapon = newWeapon;
        currentWeapon.combatUI.Initialize(newWeapon);
    }
}
