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

        [PropertyRange(1, nameof(GetMax))]
        public int maxRanks;
        private int GetMax
        {
            get
            {
                if (skill)
                {
                    return skill.skillLevels;
                }
                return 1;
            }
        }

        public Skill skill;

        [TextArea]
        public string description;

        [ToggleGroup(nameof(hasRequirements))]
        public bool hasRequirements;

        [ToggleGroup(nameof(hasRequirements))]
        public List<I_SkillNodeRequirements> requirements;

        [ToggleGroup(nameof(hasRequirements))]
        [HideLabel, Title("Display Type"), EnumToggleButtons]
        public DisplayType requirementsDisplayType;

        [ToggleGroup(nameof(hasRequirements))]
        [ShowIf(nameof(requirementsDisplayType), DisplayType.Combine), Hide]
        public SkillNodeRequirementsConfiguration skilNodeRequirements;
    }
}