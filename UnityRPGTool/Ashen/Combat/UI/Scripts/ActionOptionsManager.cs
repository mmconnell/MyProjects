using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using Manager;

public class ActionOptionsManager : SingletonMonoBehaviour<ActionOptionsManager>
{
    [NonSerialized]
    private CombatOptionUI[] combatOptions;
    [NonSerialized]
    private I_OptionExecutor[] optionExecutors;

    [NonSerialized]
    public CombatOptionUI previouslySelected;
    [NonSerialized]
    public CombatOptionUI currentlySelected;

    [Hide, FoldoutGroup("Color Scheme"), Title("Valid")]
    public ActionOptionColorScheme validOption;
    [Hide, FoldoutGroup("Color Scheme"), Title("Invalid")]
    public ActionOptionColorScheme invalidOption;

    public void Start()
    {
        CombatOptionUI[] foundOptions = GetComponentsInChildren<CombatOptionUI>();
        combatOptions = new CombatOptionUI[CombatOptions.Count];
        foreach (CombatOptionUI optionUI in foundOptions)
        {
            combatOptions[(int)optionUI.combatOption] = optionUI;
        }
        optionExecutors = GetComponentsInChildren<I_OptionExecutor>();
    }

    public void RegisterToolManager(ToolManager toolManager)
    {
        foreach (I_OptionExecutor executor in optionExecutors)
        {
            executor.InitializeOption(toolManager);
        }
    }

    public CombatOptionUI GetCombatOptionUI(CombatOption combatOption)
    {
        return combatOptions[(int)combatOption];
    }

    public void Restart()
    {
        foreach (CombatOptionUI combatOption in combatOptions)
        {
            combatOption.Restart();
        }
    }
}
