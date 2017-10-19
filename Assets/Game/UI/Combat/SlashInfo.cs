using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class SlashInfo
{
    public LineRenderer line;

    public float startTime;

    public HashSet<Entity> damagedMonsters;

    public int fingerID;

    public bool hasEnded = false;

    public SlashInfo(LineRenderer line, int fingerID)
    {
        this.line = line;
        this.fingerID = fingerID;

        startTime = Time.time;

        damagedMonsters = new HashSet<Entity>();
    }
}
