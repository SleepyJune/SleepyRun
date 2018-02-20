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

    Player player;

    public AudioClip audioClip;

    LayerMask collisionMask;

    public override void Initialize(Weapon weapon)
    {
        this.weapon = weapon;

        slashes = new Dictionary<int, SlashInfo>();

        player = GameManager.instance.player;

        collisionMask = LayerConstants.monsterMask;
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
            DestroyMonsters(slash, previous, newPos);

            EndSlash(slash);
        }
    }

    public void EndSlash(SlashInfo slash)
    {
        slash.hasEnded = true;

        if (audioClip)
        {
            var audioSource = slash.line.gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioClip);
        }

        slashes.Remove(slash.fingerID);
        Destroy(slash.line.gameObject, .5f);
    }

    Vector3 GetForce(SlashInfo slash)
    {
        Vector3[] positions = new Vector3[slash.line.positionCount];

        slash.line.GetPositions(positions);

        Vector3 prev = Vector3.zero;
        Vector3 force = Vector3.zero;

        for (int i = 0; i < positions.Length; i++)
        {
            if (i == 0)
            {
                prev = positions[i];
                continue;
            }

            var current = positions[i];

            var delta = current - prev;

            force += delta;

            prev = current;
        }

        return force;
    }

    void DestroyMonsters(SlashInfo slash, Vector3 v1, Vector3 v2)
    {
        Vector3 delta = v2 - v1;

        var colliders = Physics.SphereCastAll(v1, weapon.weaponRadius, delta.normalized, delta.magnitude, collisionMask);

        //Debug.DrawLine(v1, v2, Color.green, 2);

        foreach (var collider in colliders)
        {
            var monster = collider.collider.gameObject.GetComponent<Monster>();

            if (monster != null && monster.canTakeDamage)// && !slash.damagedMonsters.Contains(monster))
            {
                var dir = GetForce(slash);
                dir.y = .15f;

                var force = dir * (25.0f / (Time.time - slash.startTime));

                HitInfo hitInfo = new HitInfo
                {
                    hitType = HitType.Physical,
                    source = player,
                    hitStart = v1,
                    hitEnd = v2,
                    force = force,
                    damage = weapon.damage,
                    hitParticle = weapon.particle,
                    monsterHitType = MonsterCollisionMask.All,
                };

                monster.TakeDamage(hitInfo);
                //slash.damagedMonsters.Add(monster);
            }
        }
    }

    void DestroyMonstersCapsule(SlashInfo slash, Vector3 v1, Vector3 v2)
    {
        Vector3 delta = v2 - v1;

        Debug.Log("Vector: " + delta);

        var colliders = Physics.OverlapCapsule(v1, v2, weapon.weaponRadius, collisionMask);
        
        //Debug.DrawLine(v1, v2, Color.green, 2);

        foreach (var collider in colliders)
        {
            var monster = collider.gameObject.GetComponent<Monster>();
            
            if (monster != null && monster.canTakeDamage)// && !slash.damagedMonsters.Contains(monster))
            {
                var dir = GetForce(slash);
                dir.y = .15f;
                
                var force = dir * (25.0f / (Time.time - slash.startTime));

                HitInfo hitInfo = new HitInfo
                {
                    hitType = HitType.Physical,
                    source = player,
                    hitStart = v1,
                    hitEnd = v2,
                    force = force,
                    damage = weapon.damage,
                    hitParticle = weapon.particle,
                    monsterHitType = MonsterCollisionMask.All,
                };

                monster.TakeDamage(hitInfo);
                //slash.damagedMonsters.Add(monster);
            }
        }
    }

    void DestroyMonsters2(SlashInfo slash, Vector3 v1, Vector3 v2)
    {                
        foreach (var monster in GameManager.instance.monsterManager.monsters.Values)
        {
            if (!monster.isDead)// && !slash.damagedMonsters.Contains(monster))
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
                        hitType = HitType.Physical,
                        source = player,
                        hitStart = v1,
                        hitEnd = v2,
                        force = force,
                        damage = weapon.damage,
                        hitParticle = weapon.particle,
                        monsterHitType = MonsterCollisionMask.All,
                    };

                    monster.TakeDamage(hitInfo);
                    //slash.damagedMonsters.Add(monster);
                }
            }
        }
    }
}
