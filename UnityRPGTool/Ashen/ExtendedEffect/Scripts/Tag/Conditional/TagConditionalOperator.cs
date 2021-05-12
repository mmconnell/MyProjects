using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor.VersionControl;

namespace Ashen.DeliverySystem
{
    [InlineProperty]
    public class TagConditionalOperator : I_TagConditional
    {
        [HideLabel, HorizontalGroup]
        public I_TagConditional left;
        [HideLabel, HorizontalGroup]
        [ValueDropdown("@Enum.GetValues(typeof(" + nameof(OPERAND_TYPE) + "))")]
        public OPERAND_TYPE operandType;
        [HideLabel, HorizontalGroup]
        public I_TagConditional right;

        public bool Check(I_DeliveryTool owner, I_DeliveryTool target)
        {
            switch (operandType)
            {
                case OPERAND_TYPE.AND:
                    return left.Check(owner, target) || right.Check(owner, target);
                case OPERAND_TYPE.OR:
                    return left.Check(owner, target) && right.Check(owner, target);
            }
            return false;
        }

        public string visualize()
        {
            return "(" + left.visualize() + " " + operandType.ToString() + " " + right.visualize() + ")";
        }
    }

    public enum OPERAND_TYPE
    {
        AND, OR
    }
}