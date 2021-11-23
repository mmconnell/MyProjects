using Manager;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAttributeTool : A_EnumeratedTool<TargetAttributeTool>
{
    private Target[] defaultTargets;
    private TargetRange[] defaultTargetRanges;
    private List<AbilityTag>[] defaultTags;

    [ShowInInspector]
    private Target[] currentTargets;
    [ShowInInspector]
    private TargetRange[] currentRanges;
    [ShowInInspector]
    private List<AbilityTag>[] currentTags;

    private List<TargetChange>[] targetShifts;
    private List<TargetRangeChange>[] targetRangeShifts;
    private List<AbilityTagChange>[] abilityTagShifts;

    [OdinSerialize]
    private TargetAttributeToolConfiguration targetAttributeToolConfiguration = default;
    private TargetAttributeToolConfiguration TargetAttributeToolConfiguration
    {
        get
        {
            if (targetAttributeToolConfiguration == null)
            {
                return DefaultValues.Instance.defaultTargetAttributeToolConfiguration;
            }
            return targetAttributeToolConfiguration;
        }
    }

    public void Initialize(TargetAttributeToolConfiguration config)
    {
        targetAttributeToolConfiguration = config;
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        int size = TargetAttributes.Count;
        targetShifts = new List<TargetChange>[size];
        currentTargets = new Target[size];
        defaultTargets = new Target[size];
        targetRangeShifts = new List<TargetRangeChange>[size];
        currentRanges = new TargetRange[size];
        defaultTargetRanges = new TargetRange[size];
        abilityTagShifts = new List<AbilityTagChange>[size];
        currentTags = new List<AbilityTag>[size];
        defaultTags = new List<AbilityTag>[size];
        for (int x = 0; x < size; x++)
        {
            if (TargetAttributeToolConfiguration.DefaultTargets.TryGetValue(TargetAttributes.Instance[x], out Target foundTarget)) {
                currentTargets[x] = foundTarget;
            }
            else
            {
                currentTargets[x] = Targets.Instance.defaultTarget;
            }
            defaultTargets[x] = currentTargets[x];
            targetShifts[x] = new List<TargetChange>();
            if (TargetAttributeToolConfiguration.DefaultTargetRanges.TryGetValue(TargetAttributes.Instance[x], out TargetRange range))
            {
                currentRanges[x] = range;
            }
            else
            {
                currentRanges[x] = TargetRange.MELEE;
            }
            defaultTargetRanges[x] = currentRanges[x];
            targetRangeShifts[x] = new List<TargetRangeChange>();
            if (TargetAttributeToolConfiguration.DefaultAbilityTags.TryGetValue(TargetAttributes.Instance[x], out List<AbilityTag> tags))
            {
                currentTags[x] = tags;
            }
            else
            {
                currentTags[x] = AbilityTags.Instance.defaultAbilityTags;
            }
            defaultTags[x] = currentTags[x];
            abilityTagShifts[x] = new List<AbilityTagChange>();
        }
    }

    public Target GetTarget(TargetAttribute attribute)
    {
        if (currentTargets == null)
        {
            return Targets.Instance.defaultTarget;
        }
        return currentTargets[(int)attribute];
    }

    public void AddTargetShift(TargetAttribute attribute, int priority, string source, Target target)
    {
        targetShifts[(int)attribute].Add(new TargetChange
        {
            priority = priority,
            source = source,
            target = target
        });
        OnChangeTarget(attribute);
    }

    public void RemoveTargetShift(TargetAttribute attribute, string source)
    {
        int index = (int)attribute;
        List<TargetChange> changes = targetShifts[index];
        for (int x = 0; x < changes.Count; x++)
        {
            if (changes[x].source == source)
            {
                changes.RemoveAt(x);
                x--;
            }
        }
        OnChangeTarget(attribute);
    }

    public void OnChangeTarget(TargetAttribute attribute)
    {
        int index = (int)attribute;
        List<TargetChange> changes = targetShifts[index];
        if (changes.Count == 0)
        {
            currentTargets[index] = defaultTargets[index];
            return;
        }
        TargetChange? current = null;
        foreach (TargetChange targetChange in changes)
        {
            if (current == null || targetChange.priority < current.Value.priority)
            {
                current = targetChange;
                continue;
            }
            if (targetChange.priority == current.Value.priority)
            {
                if (targetChange.source.CompareTo(current.Value.source) < 0)
                {
                    current = targetChange;
                }
            }
        }
        currentTargets[index] = current.Value.target;
    }

    public TargetRange GetRange(TargetAttribute attribute)
    {
        if (currentRanges == null)
        {
            return TargetRange.MELEE;
        }
        return currentRanges[(int)attribute];
    }

    public void AddRangeShift(TargetAttribute attribute, int priority, string source, TargetRange range)
    {
        targetRangeShifts[(int)attribute].Add(new TargetRangeChange
        {
            priority = priority,
            source = source,
            target = range
        });
        OnChangeRange(attribute);
    }

    public void RemoveRangeShift(TargetAttribute attribute, string source)
    {
        int index = (int)attribute;
        List<TargetRangeChange> changes = targetRangeShifts[index];
        for (int x = 0; x < changes.Count; x++)
        {
            if (changes[x].source == source)
            {
                changes.RemoveAt(x);
                x--;
            }
        }
        OnChangeRange(attribute);
    }

    public void OnChangeRange(TargetAttribute attribute)
    {
        int index = (int)attribute;
        List<TargetRangeChange> changes = targetRangeShifts[index];
        if (changes.Count == 0)
        {
            currentRanges[index] = defaultTargetRanges[index];
            return;
        }
        TargetRangeChange? current = null;
        foreach (TargetRangeChange rangeChange in changes)
        {
            if (current == null || rangeChange.priority < current.Value.priority)
            {
                current = rangeChange;
                continue;
            }
            if (rangeChange.priority == current.Value.priority)
            {
                if (rangeChange.source.CompareTo(current.Value.source) < 0)
                {
                    current = rangeChange;
                }
            }
        }
        currentRanges[index] = current.Value.target;
    }

    public List<AbilityTag> GetAbilityTag(TargetAttribute attribute)
    {
        if (currentTags == null)
        {
            return AbilityTags.Instance.defaultAbilityTags;
        }
        return currentTags[(int)attribute];
    }

    public void AddAbilityTagShift(TargetAttribute attribute, int priority, string source, List<AbilityTag> tags, bool overwrite = false)
    {
        abilityTagShifts[(int)attribute].Add(new AbilityTagChange
        {
            priority = priority,
            source = source,
            tags = tags,
            overwrite = overwrite,
        });
        OnChangeAbilityTags(attribute);
    }

    public void RemoveAbilityTagShift(TargetAttribute attribute, string source)
    {
        int index = (int)attribute;
        List<TargetRangeChange> changes = targetRangeShifts[index];
        for (int x = 0; x < changes.Count; x++)
        {
            if (changes[x].source == source)
            {
                changes.RemoveAt(x);
                x--;
            }
        }
        OnChangeAbilityTags(attribute);
    }

    public void OnChangeAbilityTags(TargetAttribute attribute)
    {
        int index = (int)attribute;
        List<AbilityTagChange> changes = abilityTagShifts[index];
        if (changes.Count == 0)
        {
            currentTags[index] = defaultTags[index];
            return;
        }
        HashSet<AbilityTag> currentTagList = new HashSet<AbilityTag>();
        foreach (AbilityTag tag in defaultTags[index])
        {
            currentTagList.Add(tag);
        }
        changes.Sort((a, b) => {
            if (a.priority == b.priority)
            {
                return string.Compare(a.source, b.source);
            }
            return b.priority - a.priority;
        });
        foreach (AbilityTagChange abilityTagChange in changes)
        {
            if (abilityTagChange.overwrite)
            {
                currentTagList.Clear();
            }
            foreach (AbilityTag tag in abilityTagChange.tags)
            {
                currentTagList.Add(tag);
            }
        }
        List<AbilityTag> abilityTagList = new List<AbilityTag>();
        abilityTagList.AddRange(currentTagList);
        currentTags[index] = abilityTagList;
    }
}
