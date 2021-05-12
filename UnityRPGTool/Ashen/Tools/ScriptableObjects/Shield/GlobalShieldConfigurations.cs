using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalShieldConfigurations : SingletonScriptableObject<GlobalShieldConfigurations>
{
    [Hide, Title("Sort"), ListDrawerSettings(Expanded =true)]
    public List<ShieldSortStruct> shieldSorting;
}
