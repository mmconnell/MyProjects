using Sirenix.OdinInspector;
using Manager;
using System.Collections.Generic;

namespace Ashen.SkillTree
{
    public class OtherSkillNodeRequirement : I_SkillNodeRequirements
    {
        public SkillNode skillNode;
        //[ShowIf("@" + nameof(Max) + " > 1")]
        [PropertyRange(1, nameof(Max))]
        public int levelRequired = 1;

        [AutoPopulate]
        public List<SkillNodeLineConfiguration> lineConfigurations;

        [Title("Requirement Value"), HideLabel]
        public SkillNodeRequirementsConfiguration requirementConfiguration;

        public int Max
        {
            get
            {
                return skillNode.maxRanks;
            }
        }

        public bool RequirementsMet(SkillTreeTool skillTreeTool)
        {
            return skillTreeTool.GetCurrentLevel(skillNode) >= levelRequired;
        }
    }
}