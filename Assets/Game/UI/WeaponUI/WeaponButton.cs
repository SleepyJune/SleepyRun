using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{    
    public GameObject weaponItemTemplate;

    public Transform weaponList;

    public Image portraitImage;

    List<WeaponItem> weaponItems = new List<WeaponItem>();

    CanvasGroup weaponListCanvasGroup;

    Vector2 homePosition;

    //Image weaponImage;

    Weapon[] weapons;    

    bool isDragged;
        
    bool pointerInButton = false;
        
    Transform playerPortraitTransform;

    Animator anim;

    ComboManager comboManager;
    WeaponManager weaponManager;
        
    void Start()
    {
        playerPortraitTransform = transform.parent;

        comboManager = GameManager.instance.comboManager;
        weaponManager = GameManager.instance.weaponManager;

        anim = playerPortraitTransform.GetComponent<Animator>();

        weapons = GameManager.instance.weaponManager.weapons;

        //weaponList = transform.parent.Find("WeaponList");
        //weaponImage = transform.Find("WeaponImage").GetComponent<Image>();
        //weaponImage.sprite = GameManager.instance.weaponManager.defaultWeapon.facePortrait;

        anim.runtimeAnimatorController = GameManager.instance.weaponManager.defaultWeapon.facePortraitAC;
        portraitImage.sprite = GameManager.instance.weaponManager.defaultWeapon.facePortrait;

        weaponListCanvasGroup = weaponList.GetComponent<CanvasGroup>();

        foreach (var weapon in weapons)
        {            
            var newWeapon = Instantiate(weaponItemTemplate, weaponList);
            newWeapon.transform.Find("Mask/Image").GetComponent<Image>().sprite = weapon.image;

            var newMenuItem = newWeapon.transform.Find("Mask").GetComponent<WeaponItem>();
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

        //weaponImage.sprite = GameManager.instance.weaponManager.currentWeapon.facePortrait;

        anim.runtimeAnimatorController = GameManager.instance.weaponManager.currentWeapon.facePortraitAC;
        portraitImage.sprite = GameManager.instance.weaponManager.currentWeapon.facePortrait;
    }

    public void OnUltimateButtonPressed()
    {
        if (true)//comboManager.charged)
        {
            weaponManager.UseUltimate();
        }
    }
}
