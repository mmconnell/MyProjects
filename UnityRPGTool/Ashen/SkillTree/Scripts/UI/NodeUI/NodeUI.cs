using System;
using Sirenix.OdinInspector;

namespace Ashen.SkillTree
{
    [Serializable]
    public class NodeUI
    {
        [HideLabel, EnumToggleButtons]
        public NodeUiType type;
        [PropertyRange(1, 7)]
        public int space = 1;
        [ShowIf(nameof(type), Value = NodeUiType.SkillNode)]
        public SkillNode skillNode;

        public enum NodeUiType
        {
            Empty, SkillNode
        }
    }
}