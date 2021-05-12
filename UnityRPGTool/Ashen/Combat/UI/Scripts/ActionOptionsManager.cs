using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionOptionsManager : SingletonMonoBehaviour<ActionOptionsManager>
{
    public CombatOptionUI first;

    public List<CombatOptionUI> combatOptions;

    [NonSerialized]
    public CombatOptionUI previouslySelected;
    [NonSerialized]
    public CombatOptionUI currentlySelected;

    public void Restart()
    {
        foreach (CombatOptionUI combatOption in combatOptions)
        {
            combatOption.Restart();
        }
    }
}
