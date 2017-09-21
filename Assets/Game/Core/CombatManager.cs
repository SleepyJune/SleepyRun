using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {

    Dictionary<int, TouchInput> inputs;
    Dictionary<int, LineRenderer> lines;

    public LineRenderer linePrefab;

    public float updateFrequency;

    void Start()
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

            //newLine.transform.eulerAngles = new Vector3(60, 0, 0);

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
            
            line.positionCount = line.positionCount+1;
            line.SetPosition(line.positionCount-1, pos);

            CheckEachSegment(line);

            lines.Remove(touch.fingerId);
            Destroy(line.gameObject, .5f);
        }
    }

    void CheckEachSegment(LineRenderer line)
    {
        Vector3 lastPos = line.GetPosition(0);
        for (int i = 0; i < line.positionCount; i++)
        {
            if (i == 0)
            {                
                continue;
            }

            var currentPos = line.GetPosition(i);

            DestroyMonsters(lastPos, currentPos);

            lastPos = currentPos;
        }
    }

    void DestroyMonsters(Vector3 v1, Vector3 v2)
    {
        foreach(var monster in GameManager.instance.monsterManager.monsters.Values)
        {
            var monsterPos = monster.transform.position;

            var proj = monsterPos.ProjectPoint2DOnLineSegment(v1, v2);
            var dist = Vector3.Distance(proj, monsterPos);

            //Debug.Log(dist);


            if (dist <= 1)
            {
                GameManager.instance.comboManager.IncreaseComboCount();                
                monster.Death();
            }
        }
    }

    void OnDestroy()
    {
        TouchInputManager.instance.touchStart -= OnTouchStart;
        TouchInputManager.instance.touchMove -= OnTouchMove;
        TouchInputManager.instance.touchEnd -= OnTouchEnd;
    }
}
