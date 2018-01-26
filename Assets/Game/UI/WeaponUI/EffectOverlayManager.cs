using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class EffectOverlayManager : MonoBehaviour
{
    public GameObject[] effectPrefabs;

    Dictionary<string, GameObject> effectOverlay = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> effectOverlayDatabase = new Dictionary<string, GameObject>();

    void Awake()
    {
        foreach(var prefab in effectPrefabs)
        {
            effectOverlayDatabase.Add(prefab.name, prefab);
        }
    }

    public void AddEffectOverlay(string key)
    {
        if (effectOverlayDatabase.ContainsKey(key))
        {
            var prefab = effectOverlayDatabase[key];

            var newOverlay = Instantiate(prefab);
            newOverlay.transform.SetParent(transform, false);

            GameObject currentOverlay;
            if (effectOverlay.TryGetValue(key, out currentOverlay))
            {                
                if (currentOverlay == null)
                {
                    effectOverlay[key] = newOverlay;
                }
            }
            else
            {
                effectOverlay.Add(key, newOverlay);
            }
            
        }        
    }

    public void RemoveEffectOverlay(string key)
    {
        GameObject overlay;

        if(effectOverlay.TryGetValue(key, out overlay))
        {
            if (overlay != null)
            {
                Destroy(overlay);
            }
        }        

        effectOverlay.Remove(key);
    }
}
