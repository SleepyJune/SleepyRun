using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class WeaponButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, IDragHandler, IEndDragHandler
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

            weaponItems.Add(newMenuItem);
        }

        homePosition = transform.position;        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragged = false;

        //ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.beginDragHandler);

        weaponListCanvasGroup.alpha = 1;
        weaponListCanvasGroup.blocksRaycasts = true;

        foreach(var weapon in weaponItems)
        {
            weapon.background.color = new Color(1,1,1,.1f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = new Vector2(transform.position.x, eventData.position.y);

        isDragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        weaponListCanvasGroup.alpha = 0;
        weaponListCanvasGroup.blocksRaycasts = false;

        //transform.position = homePosition;

        weaponImage.sprite = GameManager.instance.weaponManager.currentWeapon.image;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragged)
        {
            weaponListCanvasGroup.alpha = 0;
            weaponListCanvasGroup.blocksRaycasts = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragged)
        {
            weaponListCanvasGroup.alpha = 0;
            weaponListCanvasGroup.blocksRaycasts = false;
        }
    }
}
