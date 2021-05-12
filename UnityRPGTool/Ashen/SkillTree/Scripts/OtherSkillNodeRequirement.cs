using Sirenix.OdinInspector;
using Manager;

namespace Ashen.SkillTree
{
    public class OtherSkillNodeRequirement : I_SkillNodeRequirements
    {
        public SkillNode skillNode;
        //[ShowIf("@" + nameof(Max) + " > 1")]
        [PropertyRange(1, nameof(Max))]
        public int levelRequired = 1;

        public int Max
        {
            get
            {
                if (skillNode == null)
                {
                    return 1;
                }
                if (skillNode.skillNodeEffectBuilder == null)
                {
                    return 1;
                }
                return skillNode.skillNodeEffectBuilder.Count;
            }
        }

        public bool RequirementsMet(SkillTreeTool skillTreeTool)
        {
            return skillTreeTool.GetCurrentLevel(skillNode) >= levelRequired;
        }
    }
}