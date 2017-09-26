using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class WeaponButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Weapon[] weapons;
    public GameObject weaponItemTemplate;

    List<WeaponItem> weaponItems = new List<WeaponItem>();

    Transform weaponList;
    CanvasGroup weaponListCanvasGroup;

    Vector2 homePosition;

    void Start()
    {
        weaponList = transform.parent.Find("WeaponList");
        weaponListCanvasGroup = weaponList.GetComponent<CanvasGroup>();

        foreach (var weapon in weapons)
        {            
            var newWeapon = Instantiate(weaponItemTemplate, weaponList);
            newWeapon.GetComponent<Image>().sprite = weapon.sprite;

            var newMenuItem = newWeapon.GetComponent<WeaponItem>();
            newMenuItem.weapon = weapon;

            weaponItems.Add(newMenuItem);
        }

        homePosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {        
        weaponListCanvasGroup.alpha = 1;
        weaponListCanvasGroup.blocksRaycasts = true;

        foreach(var weapon in weaponItems)
        {
            weapon.image.color = new Color(1,1,1,.5f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = new Vector2(transform.position.x, eventData.position.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        weaponListCanvasGroup.alpha = 0;
        weaponListCanvasGroup.blocksRaycasts = false;

        transform.position = homePosition;
    }
}
