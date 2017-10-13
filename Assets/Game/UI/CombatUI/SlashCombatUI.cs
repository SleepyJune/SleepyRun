using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SlashCombatUI : CombatUI
{
    Dictionary<int, TouchInput> inputs;
    Dictionary<int, LineRenderer> lines;

    public LineRenderer linePrefab;
    
    //public GameObject testObject;
            
    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;

        inputs = TouchInputManager.instance.inputs;
        lines = new Dictionary<int, LineRenderer>();
                
        GameManager.instance.touchInputManager.touchStart += OnTouchStart;
        GameManager.instance.touchInputManager.touchMove += OnTouchMove;
        GameManager.instance.touchInputManager.touchEnd += OnTouchEnd;
    }

    private void OnTouchStart(Touch touch)
    {
        if (Time.time - weapon.lastAttackTime > 1/weapon.attackFrequency)
        {
            if (!lines.ContainsKey(touch.fingerId))
            {
                var newLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

                var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

                //newLine.SetPosition(0, touch.position); //record current mouse position
                newLine.SetPosition(0, pos);


                //newLine.transform.eulerAngles = new Vector3(60, 0, 0);

                lines.Add(touch.fingerId, newLine);

                weapon.lastAttackTime = Time.time;
            }
        }
    }

    private void OnTouchMove(Touch touch)
    {
        LineRenderer line;
        if (lines.TryGetValue(touch.fingerId, out line))
        {
            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

            var start = line.GetPosition(0);

            var delta = (pos - start);

            if(delta.magnitude > weapon.maxSlashRange)
            {
                OnTouchEnd(touch);
            }
        }
    }

    private void OnTouchEnd(Touch touch)
    {
        LineRenderer line;
        if (lines.TryGetValue(touch.fingerId, out line))
        {
            var start = line.GetPosition(0);
            /*start = GameManager.instance.GetTouchPosition(start, 1f);
            line.SetPosition(0, start);*/

            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);
            
            var delta = (pos-start);

            var end = start + delta.normalized 
                * Math.Max(Math.Min(delta.magnitude, weapon.maxSlashRange), weapon.minSlashRange);

            //line.SetPosition(line.positionCount - 1, end);
            line.positionCount = line.positionCount + 1;
            line.SetPosition(line.positionCount - 1, start + delta.normalized * (delta.magnitude/2));

            line.positionCount = line.positionCount + 1;
            line.SetPosition(line.positionCount - 1, end);
                        
            ExecuteAttack(line);

            lines.Remove(touch.fingerId);
            Destroy(line.gameObject, .5f);
        }
    }
        
    protected virtual void ExecuteAttack(LineRenderer line)
    {
        var start = line.GetPosition(0);
        var end = line.GetPosition(line.positionCount - 1);

        var damage = weapon.GetDamage(start, end);
        line.colorGradient = weapon.GetSlashGradient(damage);

        Debug.Log("Total Damage: " + damage);

        DestroyMonsters(start, end, damage);
    }

    void DestroyMonsters(Vector3 v1, Vector3 v2, int damage)
    {
        foreach (var monster in GameManager.instance.monsterManager.monsters.Values)
        {
            var monsterPos = monster.transform.position;
            monsterPos.y = 0;

            var proj = monsterPos.ProjectPoint2DOnLineSegment(v1, v2);
            var dist = Vector3.Distance(proj, monsterPos);

            var diff = (v2 - v1);
            float length = diff.magnitude;
            
            var verticalRadius = (Math.Abs(diff.z) / length) * 1;
            var horizontalRadius = (Math.Abs(diff.x) / length) * weapon.weaponRadius;

            //Debug.Log((verticalRadius + horizontalRadius) * .5f);

            if (dist <= (verticalRadius + horizontalRadius) * .5f)
            {
                //var test = Instantiate(testObject, proj + new Vector3(0, monster.transform.position.y, 0), Quaternion.identity);
                //Destroy(test, 1);

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
                    damage = damage,
                    hitParticle = weapon.particle
                };

                monster.TakeDamage(hitInfo);
            }
        }
    }

    public override void End()
    {
        TouchInputManager.instance.touchStart -= OnTouchStart;
        TouchInputManager.instance.touchMove -= OnTouchMove;
        TouchInputManager.instance.touchEnd -= OnTouchEnd;
    }
}
