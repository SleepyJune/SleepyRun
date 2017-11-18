using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HorizontalSwipeUI : CombatUI
{
    public Spell spell;

    public Vector3 startOffset;

    Vector2 startPosition;
    int touchID;

    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;     
    }

    public override void OnTouchStart(Touch touch)
    {
        startPosition = touch.position;
        touchID = touch.fingerId;
    }

    public override void OnTouchEnd(Touch touch)
    {
        if (touch.fingerId == touchID)
        {
            var endPosition = touch.position;
            
            var playerTransform = GameManager.instance.player.transform;
            var direction = startPosition.x <= endPosition.x ? new Vector3(0, 0, 0) : new Vector3(0, 0, 180);

            //var direction = new Vector3(0, 0, 0);
            var newSpell = 
                Instantiate(
                    spell.gameObject,
                    playerTransform.position + startOffset, 
                    Quaternion.Euler(direction),
                    playerTransform);
        }
    }
}
