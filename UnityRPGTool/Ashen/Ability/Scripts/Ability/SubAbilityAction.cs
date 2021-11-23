using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static AbilityBuilder;
using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Manager;
using Ashen.EquationSystem;

public class SubAbilityAction : I_AbilityAction
{
    public AbilityAction parentAction;

    public SubAbilityRelativeTarget relativeTarget;
    public TargetParty targetParty;

    private TargetAttribute targetAttribute;
    public TargetAttribute TargetAttribute
    {
        set
        {
            targetAttribute = value;
        }
    }

    private bool doOverrideTarget;
    private Target overrideTarget;
    public Target OverrideTarget
    {
        set
        {
            doOverrideTarget = true;
            overrideTarget = value;
        }
    }

    private bool doOverrideRange;
    private TargetRange overrideRange;
    public TargetRange OverrideRange
    {
        set
        {
            doOverrideRange = true;
            overrideRange = value;
        }
    }

    private bool doOverrideAbilityTags;
    private List<AbilityTag> overrideAbilityTags;
    public List<AbilityTag> OverrideAbilityTags
    {
        set
        {
            doOverrideAbilityTags = true;
            overrideAbilityTags = value;
        }
    }

    private List<AbilityTag> addAbilityTags;
    public List<AbilityTag> AddAbilityTags
    {
        set
        {
            doOverrideAbilityTags = false;
            addAbilityTags = value;
        }
    }

    public AbilitySpeedCategory speedCategory;

    public DeliveryPackBuilder deliveryPack;
    public GameObject animation;

    public RelativeSpeed relativeSpeed;
    public I_Equation speedFactor;

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

    public string GetName()
    {
        return null;
    }

    public TargetParty GetTargetParty()
    {
        switch (relativeTarget)
        {
            case SubAbilityRelativeTarget.Self:
                return TargetParty.ALLY;
            case SubAbilityRelativeTarget.Target:
                return parentAction.targetParty;
            case SubAbilityRelativeTarget.Random:
                return targetParty;
        }
        return targetParty;
    }

    public I_Equation GetSpeedFactor()
    {
        return speedFactor;
    }

    public Target GetTargetType(ToolManager toolManager)
    {
        if (doOverrideTarget)
        {
            return overrideTarget;
        }
        if (targetAttribute != null)
        {
            TargetAttributeTool targetTool = toolManager.Get<TargetAttributeTool>();
            return targetTool.GetTarget(targetAttribute);
        }
        return parentAction.GetTargetType(toolManager);
    }

    public TargetRange GetTargetRange(ToolManager toolManager)
    {
        if (doOverrideRange)
        {
            return overrideRange;
        }
        if (targetAttribute != null)
        {
            TargetAttributeTool targetTool = toolManager.Get<TargetAttributeTool>();
            return targetTool.GetRange(targetAttribute);
        }
        return parentAction.GetTargetRange(toolManager);
    }

    public List<AbilityTag> GetAbilityTags(ToolManager toolManager)
    {
        List<AbilityTag> abilityTags = new List<AbilityTag>();
        if (doOverrideAbilityTags)
        {
            if (overrideAbilityTags != null)
            {
                abilityTags.AddRange(overrideAbilityTags);
            }
        }
        else if (targetAttribute != null)
        {
            TargetAttributeTool targetTool = toolManager.Get<TargetAttributeTool>();
            abilityTags.AddRange(targetTool.GetAbilityTag(targetAttribute));
        }
        else
        {
            abilityTags.AddRange(parentAction.GetAbilityTags(toolManager));
        }
        if (addAbilityTags != null)
        {
            abilityTags.AddRange(addAbilityTags);
        }
        return abilityTags;
    }

    public AbilitySpeedCategory GetSpeedCategory()
    {
        switch (relativeSpeed)
        {
            case RelativeSpeed.After:
            case RelativeSpeed.Before:
                return parentAction.GetSpeedCategory();
            case RelativeSpeed.Unique:
                return speedCategory;
            default:
                if (speedCategory == null)
                {
                    return parentAction.GetSpeedCategory();
                }
                return speedCategory;
        }
    }

    public bool IsValid(ToolManager toolManager)
    {
        return true;
    }

    public int GetResourceChange(ResourceValue resourceValue, ToolManager toolManager)
    {
        return 0;
    }

    public enum SubAbilityRelativeTarget
    {
        Target, Self, Random
    }

    public enum RelativeSpeed
    {
        Before, After, Unique
    }
}
