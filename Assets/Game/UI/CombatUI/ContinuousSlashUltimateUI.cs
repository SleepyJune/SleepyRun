using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class ContinuousSlashUltimateUI : CombatUI
{
    Dictionary<int, TouchInput> inputs;

    List<Vector3> lines;

    public LineRenderer linePrefab;

    public GameObject model;

    GameObject modelObject;

    int fingerID = -999;

    LineRenderer testLine;

    Rigidbody modelRigidBody;
    
    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;

        inputs = TouchInputManager.instance.inputs;
                
        fingerID = -999;
    }

    public override void OnTouchStart(Touch touch)
    {
        if (fingerID == -999)
        {
            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

            lines = new List<Vector3>();
            lines.Add(pos);

            modelObject = Instantiate(model, pos, Quaternion.identity);
            modelObject.transform.SetParent(GameManager.instance.player.transform);
            modelRigidBody = modelObject.GetComponent<Rigidbody>();

            fingerID = touch.fingerId;

            testLine = Instantiate(linePrefab);
            testLine.SetPosition(0, pos);
        }
    }

    public override void OnUpdate()
    {
        if (modelObject)
        {
            //var pos = modelObject.transform.position + modelObject.transform.forward * 5;
            //DestroyMonsters(modelObject.transform.position, pos);

            if(fingerID != -999)
            {
                var pos = GameManager.instance.GetTouchPosition(TouchInputManager.instance.inputs[fingerID].position, 1f);
                modelObject.transform.position = pos;

                /*var diff = pos - modelObject.transform.position;
                    Debug.Log(diff);
                    modelRigidBody.velocity = diff / Time.deltaTime;*/

                var diff = pos - modelObject.transform.position;
                


                //modelRigidBody.velocity = diff / Time.deltaTime;
                //modelRigidBody.MovePosition(modelObject.transform.position + diff.normalized * 50 * Time.deltaTime);
            }
        }        
    }

    public override void OnTouchMove(Touch touch)
    {
        if (touch.fingerId == fingerID)
        {
            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

            //var diff = pos - modelObject.transform.position;

            //modelRigidBody.velocity = diff / Time.deltaTime;
            //modelRigidBody.MovePosition(modelObject.transform.position + diff.normalized * 50);

            DestroyMonsters(modelObject.transform.position, pos);

            modelObject.transform.position = pos;

        }
    }

    public override void OnTouchEnd(Touch touch)
    {
        if (touch.fingerId == fingerID)
        {
            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);
            lines.Add(pos);

            testLine.positionCount = testLine.positionCount + 1;
            testLine.SetPosition(testLine.positionCount - 1, pos);
            Destroy(testLine.gameObject, .5f);

            fingerID = -999;
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
                    /*var dir = (v2 - v1).normalized;
                    var hitParticle = Instantiate(
                            particleOnHit,
                            monster.transform.position + new Vector3(0, .5f, 0),
                            Quaternion.LookRotation(-dir));*/

                    var dir = (v2 - v1);
                    dir.y = .15f;

                    var force = dir.normalized * 500;

                    HitInfo hitInfo = new HitInfo
                    {
                        hitStart = v1,
                        hitEnd = v2,
                        force = force,
                        damage = weapon.damage
                    };

                    monster.TakeDamage(hitInfo);
                }
            }
        }
    }

    public override void End()
    {

    }
}
