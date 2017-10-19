using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponButton : MonoBehaviour
{    
    public GameObject weaponItemTemplate;

    List<WeaponItem> weaponItems = new List<WeaponItem>();

    Transform weaponList;
    CanvasGroup weaponListCanvasGroup;

    Vector2 homePosition;

    Image weaponImage;

    Weapon[] weapons;

    bool isDragged;

    void Start()
    {
        weapons = GameManager.instance.weaponManager.weapons;

        weaponList = transform.parent.Find("WeaponList");
        weaponImage = transform.parent.Find("Image").GetComponent<Image>();
        weaponImage.sprite = GameManager.instance.weaponManager.defaultWeapon.image;

        weaponListCanvasGroup = weaponList.GetComponent<CanvasGroup>();

        foreach (var weapon in weapons)
        {            
            var newWeapon = Instantiate(weaponItemTemplate, weaponList);
            newWeapon.transform.Find("Image").GetComponent<Image>().sprite = weapon.image;

            var newMenuItem = newWeapon.GetComponent<WeaponItem>();
            newMenuItem.weapon = weapon;
            newMenuItem.weaponButton = this;

            weaponItems.Add(newMenuItem);
        }

        homePosition = transform.position;

        GameManager.instance.touchInputManager.touchStart += HideWeaponItems;      
    }

    public void HideWeaponItems(Touch touch)
    {
        weaponListCanvasGroup.alpha = 0;
        weaponListCanvasGroup.blocksRaycasts = false;
    }

    public void OnWeaponButtonClick()
    {
        if(weaponListCanvasGroup.alpha == 0)
        {
            weaponListCanvasGroup.alpha = 1;
            weaponListCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            weaponListCanvasGroup.alpha = 0;
            weaponListCanvasGroup.blocksRaycasts = false;
        }        
    }

    public void OnWeaponSelect()
    {
        weaponListCanvasGroup.alpha = 0;
        weaponListCanvasGroup.blocksRaycasts = false;

        weaponImage.sprite = GameManager.instance.weaponManager.currentWeapon.image;
    }
}
