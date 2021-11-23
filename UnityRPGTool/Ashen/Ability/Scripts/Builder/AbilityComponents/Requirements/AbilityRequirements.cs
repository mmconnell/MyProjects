using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[Serializable]
public class AbilityRequirements
{
    [FoldoutGroup("Requirements"), Hide, ListDrawerSettings(Expanded = true)]
    public List<I_AbilityRequirement> requirements;

    [ToggleGroup(nameof(hasCosts))]
    public bool hasCosts;
    [ToggleGroup(nameof(hasCosts)), Hide]
    public AbilityRequirementsCost costs;

    [ToggleGroup(nameof(doesGenerate))]
    public bool doesGenerate;
    [ToggleGroup(nameof(doesGenerate)), Hide]
    public AbilityRequirementsCost generates;
}

public enum AbilityCostInspector
{
    Use, Generate
}