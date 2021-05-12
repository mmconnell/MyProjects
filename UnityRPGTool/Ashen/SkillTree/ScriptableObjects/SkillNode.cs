using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Ashen.SkillTree
{
    public class SkillNode : SerializedScriptableObject
    {
        public string skillName;
        public SkillNodeCategory category;

        [OdinSerialize, Hide]
        public List<SkillNodeEffectBuilder> skillNodeEffectBuilder;

        [TextArea]
        public string description;

        public bool hasRequirements;

        [ShowIf("@" + nameof(hasRequirements))]
        public List<I_SkillNodeRequirements> requirements;
    }
}