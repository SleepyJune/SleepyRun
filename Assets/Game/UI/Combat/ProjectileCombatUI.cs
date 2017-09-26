using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ProjectileCombatUI : CombatUI
{
    Dictionary<int, TouchInput> inputs;
    Dictionary<int, LineRenderer> lines;

    public LineRenderer linePrefab;
    public LinearSpell projectilePrefab;

    public override void Initialize()
    {
        inputs = TouchInputManager.instance.inputs;
        lines = new Dictionary<int, LineRenderer>();

        GameManager.instance.touchInputManager.touchStart += OnTouchStart;
        GameManager.instance.touchInputManager.touchMove += OnTouchMove;
        GameManager.instance.touchInputManager.touchEnd += OnTouchEnd;
    }

    private void OnTouchStart(Touch touch)
    {
        if (!lines.ContainsKey(touch.fingerId))
        {
            var newLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

            newLine.SetPosition(0, pos);
            lines.Add(touch.fingerId, newLine);
        }
    }

    private void OnTouchMove(Touch touch)
    {
        if (lines.ContainsKey(touch.fingerId))
        {

        }
    }

    private void OnTouchEnd(Touch touch)
    {
        LineRenderer line;
        if (lines.TryGetValue(touch.fingerId, out line))
        {
            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

            line.positionCount = line.positionCount + 1;
            line.SetPosition(line.positionCount - 1, pos);

            var projectileObject = Instantiate(projectilePrefab.gameObject, line.GetPosition(0), Quaternion.identity);
            var projectile = projectileObject.GetComponent<LinearSpell>();

            projectile.start = line.GetPosition(0);
            projectile.end = line.GetPosition(1);
            projectile.speed = (projectile.end - projectile.start).magnitude * 300;

            projectile.SetVelocity();
            
            lines.Remove(touch.fingerId);
            Destroy(line.gameObject, .5f);
        }
    }    

    public override void Destroy()
    {
        TouchInputManager.instance.touchStart -= OnTouchStart;
        TouchInputManager.instance.touchMove -= OnTouchMove;
        TouchInputManager.instance.touchEnd -= OnTouchEnd;
    }
}
