using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Lane destinationLane = Lane.mid;
    Vector2 swipeStartPos;

    bool dragStarted = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        swipeStartPos = eventData.position;

        dragStarted = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = eventData.position;

        var endPos = eventData.position;
        var delta = (endPos - swipeStartPos);

        if (Math.Abs(delta.x) > 50)
        {
            if (GameManager.instance.isGameOver || GameManager.instance.isGamePaused)
            {
                return;
            }
            else
            {
                OnEndDrag(eventData);
            }

            dragStarted = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragStarted || GameManager.instance.isGameOver || GameManager.instance.isGamePaused)
        {
            return;
        }

        var endPos = eventData.position;

        var delta = (endPos - swipeStartPos);

        var playerPosition = GameManager.instance.player.transform.position;
        
        if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
        {
            if (GameManager.instance.player.isConfused)
            {
                delta.x *= -1;
            }

            if (delta.x > 0)
            {
                if (playerPosition.x < 0)
                {
                    destinationLane = Lane.mid;
                }
                else if (playerPosition.x >= 0)
                {
                    destinationLane = Lane.right;
                }
            }
            else
            {
                if (playerPosition.x > 0)
                {
                    destinationLane = Lane.mid;
                }
                else if (playerPosition.x <= 0)
                {
                    destinationLane = Lane.left;
                }
            }

            GameManager.instance.player.destinationLane = destinationLane;
        }
        else
        {
            if (delta.y <= 0)
            {


            }
            else
            {
                /*
                if (true)//comboManager.charged)
                {
                    weaponManager.UseUltimate();
                }*/
            }

        }
    }
}
