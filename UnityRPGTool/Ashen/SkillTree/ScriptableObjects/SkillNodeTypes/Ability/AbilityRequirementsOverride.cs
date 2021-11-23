using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[Serializable]
public class AbilityRequirementsOverride
{
    [ToggleGroup(nameof(shouldOverrideRequirements))]
    public bool shouldOverrideRequirements;
    [ToggleGroup(nameof(shouldOverrideRequirements)), EnumToggleButtons, HideLabel]
    public OverrideType overrideType;
    [ToggleGroup(nameof(shouldOverrideRequirements)), Hide, ListDrawerSettings(Expanded = true), ShowIf(nameof(overrideType), OverrideType.Replace)]
    public List<I_AbilityRequirement> requirements;
    [ToggleGroup(nameof(shouldOverrideRequirements)), Hide, ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true), ShowIf(nameof(overrideType), OverrideType.Override)]
    public List<AbilityRequirementsType> overrideRequirements;

    [ToggleGroup(nameof(shouldOverrideCosts))]
    public bool shouldOverrideCosts;
    [ToggleGroup(nameof(shouldOverrideCosts)), Hide]
    public AbilityRequirementsCost costs;

    [ToggleGroup(nameof(shouldOverrideGenerators))]
    public bool shouldOverrideGenerators;
    [ToggleGroup(nameof(shouldOverrideGenerators)), Hide]
    public AbilityRequirementsCost generates;
}