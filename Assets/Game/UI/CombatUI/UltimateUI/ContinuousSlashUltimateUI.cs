using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ContinuousSlashUltimateUI : UltimateCombatUI
{
    Dictionary<int, TouchInput> inputs;
    Dictionary<int, LineRenderer> lines;

    public LineRenderer linePrefab;

    public GameObject model;

    GameObject modelObject;

    public override void Initialize(Weapon weapon)
    {
        inputs = TouchInputManager.instance.inputs;
        lines = new Dictionary<int, LineRenderer>();

        GameManager.instance.touchInputManager.touchStart += OnTouchStart;
        GameManager.instance.touchInputManager.touchMove += OnTouchMove;
        GameManager.instance.onUpdate += OnUpdate;
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

            modelObject = Instantiate(model, pos, Quaternion.identity);
            modelObject.transform.SetParent(GameManager.instance.player.transform);
            //modelObject.GetComponent<Collider>().trigg
        }
    }

    private void OnUpdate()
    {
        if (modelObject)
        {
            var pos = modelObject.transform.position + modelObject.transform.forward * 5;
            DestroyMonsters(modelObject.transform.position, pos);
        }        
    }

    private void OnTouchMove(Touch touch)
    {
        if (lines.ContainsKey(touch.fingerId))
        {
            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);
            modelObject.transform.position = pos;
        }
    }

    private void OnTouchEnd(Touch touch)
    {
        LineRenderer line;
        if (lines.TryGetValue(touch.fingerId, out line))
        {
            //CheckEachSegment(line);

            lines.Remove(touch.fingerId);
            Destroy(line.gameObject, .5f);

            Destroy(modelObject);
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
        foreach (var monster in GameManager.instance.monsterManager.monsters.Values)
        {
            if (!monster.isDead)
            {
                var monsterPos = monster.transform.position;
                monsterPos.y = 0;

                var proj = monsterPos.ProjectPoint2DOnLineSegment(v1, v2);
                var dist = Vector3.Distance(proj, monsterPos);

                //Debug.Log(dist);


                if (dist <= .5f)
                {
                    GameManager.instance.comboManager.IncreaseComboCount();

                    /*var dir = (v2 - v1).normalized;
                    var hitParticle = Instantiate(
                            particleOnHit,
                            monster.transform.position + new Vector3(0, .5f, 0),
                            Quaternion.LookRotation(-dir));*/

                    var dir = (v2 - v1);
                    dir.y = .15f;

                    var force = dir * 50;

                    HitInfo hitInfo = new HitInfo
                    {
                        hitStart = v1,
                        hitEnd = v2,
                        force = force,
                    };

                    monster.Death(hitInfo);
                }
            }
        }
    }

    public override void Destroy()
    {
        TouchInputManager.instance.touchStart -= OnTouchStart;
        TouchInputManager.instance.touchMove -= OnTouchMove;
        TouchInputManager.instance.touchEnd -= OnTouchEnd;
    }
}
