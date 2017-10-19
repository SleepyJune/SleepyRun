using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponItem : MonoBehaviour
{
    [NonSerialized]
    public Weapon weapon;

    [NonSerialized]
    public Image background;

    [NonSerialized]
    public WeaponButton weaponButton;

    void Awake()
    {
        background = GetComponent<Image>();
    }

    public void WeaponSelect()
    {
        GameManager.instance.weaponManager.SwitchWeapons(weapon);
        weaponButton.OnWeaponSelect();
    }
}
