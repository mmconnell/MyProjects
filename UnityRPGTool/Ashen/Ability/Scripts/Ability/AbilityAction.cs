using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using Manager;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static AbilityBuilder;

public class AbilityAction : I_AbilityAction
{
    public TargetAttribute targetAttribute;

    //private bool doOverrideTarget;
    //private Target overrideTarget;
    //public Target OverrideTarget
    //{
    //    get { return overrideTarget; }
    //    set
    //    {
    //        doOverrideTarget = true;
    //        overrideTarget = value;
    //    }
    //}

    public Target customTarget;

    //private bool doOverrideRange;
    //private TargetRange overrideRange;
    //public TargetRange OverrideRange
    //{
    //    get { return overrideRange;  }
    //    set
    //    {
    //        doOverrideRange = true;
    //        overrideRange = value;
    //    }
    //}

    public TargetRange? customRange;

    //private bool doOverrideAbilityTags;
    //private List<AbilityTag> overrideAbilityTags;
    //public List<AbilityTag> OverrideAbilityTags
    //{
    //    get { return overrideAbilityTags; }
    //    set
    //    {
    //        doOverrideAbilityTags = true;
    //        overrideAbilityTags = value;
    //    }
    //}

    private bool useCustomAbilityTags;
    private List<AbilityTag> customAbilityTags;
    public List<AbilityTag> CustomAbilityTags
    {
        set
        {
            useCustomAbilityTags = true;
            customAbilityTags = value;
        }
    }

    public AbilitySpeedCategory speedCategory;
    public I_Equation speedFactor;

    public DeliveryPackBuilder deliveryPack;
    public GameObject animation;
    public TargetParty targetParty;
    public string name;
    [ShowInInspector, ReadOnly]
    private List<I_AbilityRequirement> requirements;
    public List<I_AbilityRequirement> Requirements
    {
        get
        {
            return requirements;
        }
        set
        {
            if (value == null)
            {
                requirements = null;
            }
            else
            {
                requirements = new List<I_AbilityRequirement>();
                requirements.AddRange(value);
            }
        }
    }

    public I_Equation primaryResourceCost;
    [ShowInInspector, ReadOnly]
    private I_Equation[] resourceCosts;

    public I_Equation primaryResourceGenerator;
    [ShowInInspector, ReadOnly]
    private I_Equation[] resourceGenerators;

    public void SetResourceCost(ResourceValue resourceValue, I_Equation equation)
    {
        if (resourceCosts == null)
        {
            resourceCosts = new I_Equation[ResourceValues.Count];
        }
        resourceCosts[(int)resourceValue] = equation;
    }

    public void SetResourceGeneration(ResourceValue resourceValue, I_Equation equation)
    {
        if (resourceGenerators == null)
        {
            resourceGenerators = new I_Equation[ResourceValues.Count];
        }
        resourceGenerators[(int)resourceValue] = equation;
    }

    public int GetResourceChange(ResourceValue resourceValue, ToolManager toolManager)
    {
        ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
        I_Equation cost = null;
        I_Equation generate = null;

        if (resourceValue == rvTool.AbilityResourceValue)
        {
            cost = primaryResourceCost;
            generate = primaryResourceGenerator;
        }
        else
        {
            cost = resourceCosts == null ? null : resourceCosts[(int)resourceValue];
            generate = resourceGenerators == null ? null : resourceGenerators[(int)resourceValue];
        }

        if (cost == null && generate == null)
        {
            return 0;
        }

        DeliveryTool dTool = toolManager.Get<DeliveryTool>();

        return ((cost != null) ? (int)cost.Calculate(dTool) : 0) - ((generate != null) ? (int)generate.Calculate(dTool) : 0);
    }

    [ShowInInspector, ReadOnly]
    private float?[] effectFloatArguments;

    public void SetEffectFloat(A_EffectFloatArgument argument, float value)
    {
        if (effectFloatArguments == null)
        {
            effectFloatArguments = new float?[EffectFloatArguments.Count];
            effectFloatArguments[(int)argument] = value;
        }
    }

    public void FillDeliveryArguments(DeliveryArgumentPacks deliveryArguments)
    {
        if (effectFloatArguments == null)
        {
            return;
        }
        EffectsArgumentPack effectsArguments = deliveryArguments.GetPack<EffectsArgumentPack>();
        foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
        {
            if (effectFloatArguments[(int)argument] != null)
            {
                effectsArguments.SetFloatArgument(argument, (float)effectFloatArguments[(int)argument]);
            }
        }
    }

    public DeliveryPackBuilder GetDeliveryPack()
    {
        return deliveryPack;
    }

    public GameObject GetAnimation()
    {
        return animation;
    }

    public TargetParty GetTargetParty()
    {
        return targetParty;
    }

    public I_Equation GetSpeedFactor()
    {
        return speedFactor;
    }

    public AbilitySpeedCategory GetSpeedCategory()
    {
        return speedCategory;
    }

    public string GetName()
    {
        return name;
    }

    public Target GetTargetType(ToolManager toolManager)
    {
        if (customTarget != null)
        {
            return customTarget;
        }
        TargetAttributeTool targetTool = toolManager.Get<TargetAttributeTool>();
        return targetTool.GetTarget(targetAttribute);
    }

    public TargetRange GetTargetRange(ToolManager toolManager)
    {
        if (customRange != null)
        {
            return customRange.Value;
        }
        TargetAttributeTool targetTool = toolManager.Get<TargetAttributeTool>();
        return targetTool.GetRange(targetAttribute);
    }

    public List<AbilityTag> GetAbilityTags(ToolManager toolManager)
    {
        List<AbilityTag> abilityTags = new List<AbilityTag>();
        if (useCustomAbilityTags)
        { 
            if (abilityTags != null)
            {
                abilityTags.AddRange(customAbilityTags);

            }
            return abilityTags;
        }
        TargetAttributeTool targetTool = toolManager.Get<TargetAttributeTool>();
        abilityTags.AddRange(targetTool.GetAbilityTag(targetAttribute));
        return abilityTags;
    }

    public bool IsValid(ToolManager toolManager)
    {
        if (requirements == null || requirements.Count == 0)
        {
            return true;
        }
        foreach (I_AbilityRequirement requirement in requirements)
        {
            if (!requirement.IsValid(toolManager))
            {
                return false;
            }
        }
        return true;
    }
}
