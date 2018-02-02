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
    public CombatUI currentCombatUI;

    bool usingUlt = false;

    bool weaponDisabled = false;
    
    void Start()
    {
        GameManager.instance.touchInputManager.touchStart += OnTouchStartHandler;
        GameManager.instance.touchInputManager.touchMove += OnTouchMoveHandler;
        GameManager.instance.touchInputManager.touchEnd += OnTouchEndHandler;

        if (currentWeapon == null)
        {
            SwitchWeapons(defaultWeapon);
        }        
    }

    void OnTouchStartHandler(Touch touch)
    {
        if (currentCombatUI != null && !weaponDisabled)
        {
            currentCombatUI.OnTouchStart(touch);
        }
    }

    void OnTouchMoveHandler(Touch touch)
    {
        if (currentCombatUI != null && !weaponDisabled)
        {
            currentCombatUI.OnTouchMove(touch);
        }
    }

    void OnTouchEndHandler(Touch touch)
    {
        if (currentCombatUI != null && !weaponDisabled)
        {
            currentCombatUI.OnTouchEnd(touch);
        }
    }

    void Update()
    {
        if (currentCombatUI != null)
        {
            currentCombatUI.OnUpdate();
        }
    }

    public void SwitchCombatUI(CombatUI newCombatUI)
    {
        if (newCombatUI == null)
        {
            if (currentCombatUI != null)
            {
                currentCombatUI.End();

                currentCombatUI = currentWeapon.combatUI;
                currentCombatUI.Initialize(currentWeapon);
            }
        }
        else
        {
            currentWeapon.combatUI.End();

            currentCombatUI = newCombatUI;
            currentCombatUI.Initialize(currentWeapon);
        }
    }

    public void UseUltimate()
    {
        if (GameManager.instance.player.canUseSkills)
        {
            if (!usingUlt && currentWeapon.ultimateUI != null)
            {
                usingUlt = true;

                currentWeapon.combatUI.End();

                currentCombatUI = currentWeapon.ultimateUI;
                currentCombatUI.Initialize(currentWeapon);

            }
        }
    }

    public void EndUltimate()
    {
        if (usingUlt && currentWeapon.ultimateUI)
        {
            usingUlt = false;

            currentWeapon.ultimateUI.End();

            currentCombatUI = currentWeapon.combatUI;
            currentCombatUI.Initialize(currentWeapon);

        }
    }

    public void DisableWeapon(bool disable)
    {
        /*if (currentCombatUI != null)
        {
            currentCombatUI.End();
            currentCombatUI = null;

            usingUlt = false;
        }*/

        weaponDisabled = disable;
    }

    public void SwitchWeapons(Weapon newWeapon)
    {
        if (!GameManager.instance.player.isDead)
        {
            if (currentCombatUI != null)
            {
                currentCombatUI.End();
                usingUlt = false;
            }

            currentWeapon = newWeapon;

            currentCombatUI = currentWeapon.combatUI;
            currentCombatUI.Initialize(currentWeapon);
        }
    }
}
