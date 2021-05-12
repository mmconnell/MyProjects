using UnityEngine;
using System.Collections;
using Manager;

namespace Ashen.SkillTree
{
    public interface I_SkillNodeRequirements
    {
        bool RequirementsMet(SkillTreeTool skillTreeTool);
    }
}