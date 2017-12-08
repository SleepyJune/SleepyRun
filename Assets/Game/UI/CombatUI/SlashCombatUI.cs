using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SlashCombatUI : CombatUI
{
    Dictionary<int, SlashInfo> slashes;

    public LineRenderer linePrefab;
    float currentLineLength = 0;
    //public GameObject testObject;
            
    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;

        slashes = new Dictionary<int, SlashInfo>();
    }

    public override void OnTouchStart(Touch touch)
    {
        if(true)//if (Time.time - weapon.lastAttackTime > 1/weapon.attackFrequency)
        {           
            if (!slashes.ContainsKey(touch.fingerId))
            {
                if (weapon.isDualWeapon || slashes.Count == 0) //limit multiple slashes
                {
                    var newLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

                    var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

                    //newLine.SetPosition(0, touch.position); //record current mouse position
                    newLine.SetPosition(0, pos);
                    currentLineLength = 0;

                    //newLine.transform.eulerAngles = new Vector3(60, 0, 0);

                    var slash = new SlashInfo(newLine, touch.fingerId);

                    slashes.Add(touch.fingerId, slash);

                    weapon.lastAttackTime = Time.time;

                    DelayAction.Add(()=>CheckSlashEndTime(slash), .5f);
                }
            }
        }
    }

    void CheckSlashEndTime(SlashInfo slash)
    {
        if (!slash.hasEnded)
        {
            EndSlash(slash);
        }
    }

    float GetLineLength(LineRenderer line)
    {
        Vector3[] positions = new Vector3[line.positionCount];

        line.GetPositions(positions);

        float length = 0;

        for (int i = 0; i < positions.Length; i++)
        {
            if (i > 0)
            {
                length += (positions[i] - positions[i - 1]).magnitude;
            }
        }

        return length;
    }

    Vector3 GetNewPosition(LineRenderer line, Vector3 pos)
    {
        var previous = line.GetPosition(line.positionCount - 1);
        var delta = (pos - previous);

        delta.y = 0;
        delta.z = delta.z / 2;

        var newLength = delta.magnitude + currentLineLength;

        if(newLength > weapon.maxSlashRange)
        {
            var lengthLeft = weapon.maxSlashRange - currentLineLength;
            currentLineLength = weapon.maxSlashRange;
            return previous + delta.normalized * lengthLeft;
        }
        else
        {
            currentLineLength = newLength;
            return pos;
        }
    }

    public override void OnTouchMove(Touch touch)
    {
        SlashInfo slash;
        if (slashes.TryGetValue(touch.fingerId, out slash))
        {
            LineRenderer line = slash.line;
            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

            var previous = line.GetPosition(line.positionCount - 1);

            var newPos = GetNewPosition(line, pos);               
            var delta = (newPos - previous);
            
            if(line.positionCount > 0 )//&& delta.magnitude > 1)
            {
                                
                //line.Simplify(.1f);

                //ExecuteAttack(line);

                if (newPos != pos)
                {
                    OnTouchEnd(touch);
                }
                else
                {
                    if (line.positionCount == 1)
                    {
                        var middle = previous + delta.normalized * (delta.magnitude / 2);

                        line.positionCount = line.positionCount + 1;
                        line.SetPosition(line.positionCount - 1, middle);
                    }

                    line.positionCount = line.positionCount + 1;
                    line.SetPosition(line.positionCount - 1, newPos);
                    
                    DestroyMonsters(slash, previous, newPos);
                }                
            }
        }
    }

    public override void OnTouchEnd(Touch touch)
    {
        SlashInfo slash;
        if (slashes.TryGetValue(touch.fingerId, out slash))
        {
            LineRenderer line = slash.line;

            var start = line.GetPosition(0);
            /*start = GameManager.instance.GetTouchPosition(start, 1f);
            line.SetPosition(0, start);*/

            var pos = GameManager.instance.GetTouchPosition(touch.position, 1f);

            var delta = (pos - start);

            var end = start + delta.normalized
                * Math.Max(Math.Min(delta.magnitude, weapon.maxSlashRange), weapon.minSlashRange);

            var newPos = GetNewPosition(line, pos);
            line.positionCount = line.positionCount + 1;
            line.SetPosition(line.positionCount - 1, newPos);

            var previous = line.GetPosition(line.positionCount - 2);
            DestroyMonsters(slash, previous, pos);

            EndSlash(slash);
        }
    }

    public void EndSlash(SlashInfo slash)
    {
        slash.hasEnded = true;

        slashes.Remove(slash.fingerID);
        Destroy(slash.line.gameObject, .5f);
    }

    protected virtual void ExecuteAttack(SlashInfo slash)
    {
        LineRenderer line = slash.line;

        var start = line.GetPosition(0);
        var end = line.GetPosition(line.positionCount - 1);

        var damage = weapon.GetDamage(start, end);
        line.colorGradient = weapon.GetSlashGradient(damage);

        Debug.Log("Total Damage: " + damage);

        DestroyMonsters(slash, start, end);
    }

    void DestroyMonsters(SlashInfo slash, Vector3 v1, Vector3 v2)
    {
        foreach (var monster in GameManager.instance.monsterManager.monsters.Values)
        {
            if (!slash.damagedMonsters.Contains(monster))
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

                var monsterRadius = monster.transform.localScale.x * .5f * .5f;
                
                if (dist <= (verticalRadius + horizontalRadius) * .5f + monsterRadius)
                {
                    //previous vector with tolerance, add to force
                                        

                    var dir = (v2 - v1);
                    dir.y = .15f;

                    var force = dir * 50;

                    HitInfo hitInfo = new HitInfo
                    {
                        hitStart = v1,
                        hitEnd = v2,
                        force = force,
                        damage = weapon.damage,
                        hitParticle = weapon.particle
                    };

                    monster.TakeDamage(hitInfo);
                    slash.damagedMonsters.Add(monster);
                }
            }
        }
    }
}
