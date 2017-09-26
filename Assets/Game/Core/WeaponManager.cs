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

    public void SwitchWeapons(Weapon newWeapon)
    {
        if (currentWeapon)
        {
            currentWeapon.combatUI.Destroy();
        }

        currentWeapon = newWeapon;
        currentWeapon.combatUI.Initialize();
    }
}
