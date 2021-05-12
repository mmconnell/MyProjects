using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.SkillTree
{
    public class SkillTree : SerializedScriptableObject
    {
        [AutoPopulate]
        public List<SkillNode> skillNodes;
    }
}