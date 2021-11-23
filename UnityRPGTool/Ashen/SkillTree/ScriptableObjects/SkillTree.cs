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

        public List<NodeUI> noviceLeft;
        public List<NodeUI> noviceRight;

        public List<NodeUI> veteranLeft;
        public List<NodeUI> veteranRight;

        public List<NodeUI> masterLeft;
        public List<NodeUI> masterRight;
    }
}