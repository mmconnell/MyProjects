using Ashen.DeliverySystem;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cacher", menuName = "Custom/Managers/Cacher")]
public class Cacher : SingletonScriptableObject<Cacher>
{
    private Dictionary<string, List<I_Cacheable>> caches;

    private void OnEnable()
    {
        caches = new Dictionary<string, List<I_Cacheable>>();
    }

    public void Cache(string key, I_Cacheable toCache)
    {
        List<I_Cacheable> list = null;
        if (caches.TryGetValue(key, out list))
        {
            list.Add(toCache);
        }
        else
        {
            list = new List<I_Cacheable>();
            list.Add(toCache);
            caches.Add(key, list);
        }
    }

    public void OnChange(I_DeliveryTool toolManager, string key)
    {
        List<I_Cacheable> list = null;
        if (caches.TryGetValue(key, out list))
        {
            List<I_Cacheable> cacheable = new List<I_Cacheable>(list);
            for (int x = 0; x < cacheable.Count; x++)
            {
                cacheable[x].Recalculate(toolManager, null);
            }
            list.Clear();
        }
    }
}
