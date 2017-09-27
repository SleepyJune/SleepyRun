using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

public class UltButton : MonoBehaviour
{
    ComboManager comboManager;
    WeaponManager weaponManager;
        
    void Start()
    {
        comboManager = GameManager.instance.comboManager;
        weaponManager = GameManager.instance.weaponManager;
    }

    public void OnButtonClick()
    {
        if (true)//comboManager.charged)
        {
            weaponManager.UseUltimate();
        }
    }
}
