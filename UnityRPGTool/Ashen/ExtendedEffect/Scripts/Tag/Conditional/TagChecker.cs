using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class TagChecker : I_TagConditional
    {
        [HideLabel, Title("Tag")]
        public ExtendedEffectTag tag;
        [HideLabel, Title("Negate")]
        public bool negate;

        public bool Check(I_DeliveryTool owner, I_DeliveryTool target)
        {
            DeliveryTool tDeliveryTool = target as DeliveryTool;
            if (tDeliveryTool)
            {
                StatusTool tStatusTool = tDeliveryTool.toolManager.Get<StatusTool>();
                if (tStatusTool)
                {
                    return tStatusTool.CheckStatusEffectTag(tag);
                }
            }
            return false;
        }

        public string visualize()
        {
            string visualization = "";
            if (negate)
            {
                visualization += "!";
            }
            visualization += "Has " + tag.ToString();
            return visualization;
        }
    }

}