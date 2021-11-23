using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;

public class TargetAttributeToolConfiguration : SerializedScriptableObject
{
    [OdinSerialize]
    private Dictionary<TargetAttribute, Target> defaultTargets;
    [OdinSerialize]
    private Dictionary<TargetAttribute, TargetRange> defaultTargetRanges;
    [OdinSerialize]
    private Dictionary<TargetAttribute, List<AbilityTag>> defaultAbilityTags;

    public Dictionary<TargetAttribute, Target> DefaultTargets
    {
        get
        {
            if (defaultTargets == null)
            {
                if (this == DefaultValues.Instance.defaultTargetAttributeToolConfiguration)
                {
                    return null;
                }
                return DefaultValues.Instance.defaultTargetAttributeToolConfiguration.DefaultTargets;
            }
            else
            {
                Dictionary<TargetAttribute, Target> derivedDefaultBase = new Dictionary<TargetAttribute, Target>();
                foreach (TargetAttribute statAttribute in TargetAttributes.Instance)
                {
                    if (defaultTargets.ContainsKey(statAttribute))
                    {
                        derivedDefaultBase.Add(statAttribute, defaultTargets[statAttribute]);
                    }
                    else
                    {
                        if (this != DefaultValues.Instance.defaultBaseAttributeToolConfiguration)
                        {
                            derivedDefaultBase.Add(statAttribute, DefaultValues.Instance.defaultTargetAttributeToolConfiguration.defaultTargets[statAttribute]);
                        }
                    }
                }
                return derivedDefaultBase;
            }
        }
    }

    public Dictionary<TargetAttribute, TargetRange> DefaultTargetRanges
    {
        get
        {
            if (defaultTargetRanges == null)
            {
                if (this == DefaultValues.Instance.defaultTargetAttributeToolConfiguration)
                {
                    return null;
                }
                return DefaultValues.Instance.defaultTargetAttributeToolConfiguration.DefaultTargetRanges;
            }
            else
            {
                Dictionary<TargetAttribute, TargetRange> derivedDefaultBase = new Dictionary<TargetAttribute, TargetRange>();
                foreach (TargetAttribute statAttribute in TargetAttributes.Instance)
                {
                    if (defaultTargetRanges.ContainsKey(statAttribute))
                    {
                        derivedDefaultBase.Add(statAttribute, defaultTargetRanges[statAttribute]);
                    }
                    else
                    {
                        if (DefaultValues.Instance.defaultTargetAttributeToolConfiguration.defaultTargetRanges.TryGetValue(statAttribute, out TargetRange range)) {
                            derivedDefaultBase.Add(statAttribute, range);
                        }
                        else
                        {
                            derivedDefaultBase.Add(statAttribute, TargetRange.MELEE);
                        }
                    }
                }
                return derivedDefaultBase;
            }
        }
    }

    public Dictionary<TargetAttribute, List<AbilityTag>> DefaultAbilityTags
    {
        get
        {
            if (defaultAbilityTags == null)
            {
                if (this == DefaultValues.Instance.defaultTargetAttributeToolConfiguration)
                {
                    return null;
                }
                return DefaultValues.Instance.defaultTargetAttributeToolConfiguration.DefaultAbilityTags;
            }
            else
            {
                Dictionary<TargetAttribute, List<AbilityTag>> derivedDefaultBase = new Dictionary<TargetAttribute, List<AbilityTag>>();
                foreach (TargetAttribute statAttribute in TargetAttributes.Instance)
                {
                    if (defaultAbilityTags.ContainsKey(statAttribute))
                    {
                        derivedDefaultBase.Add(statAttribute, defaultAbilityTags[statAttribute]);
                    }
                    else
                    {
                        if (DefaultValues.Instance.defaultTargetAttributeToolConfiguration.defaultAbilityTags.TryGetValue(statAttribute, out List<AbilityTag> tags))
                        {
                            derivedDefaultBase.Add(statAttribute, tags);
                        }
                        else
                        {
                            derivedDefaultBase.Add(statAttribute, AbilityTags.Instance.defaultAbilityTags);
                        }
                    }
                }
                return derivedDefaultBase;
            }
        }
    }
}
