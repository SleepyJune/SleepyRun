using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "StageEvent/StageEvent Database")]
public class StageEventDatabase : ScriptableObject
{
    public StageEvent[] allEvents = new StageEvent[0];

    public string[] allEventOptions = new string[0];
}
