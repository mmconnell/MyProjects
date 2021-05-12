using System.Collections;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using Manager;
using Sirenix.OdinInspector;
using UnityEngine;

/**
 * A character attribute defines the core values that make up the general power and effectiveness of a character
 **/
[CreateAssetMenu(fileName = nameof(BaseAttribute), menuName = "Custom/Enums/" + nameof(BaseAttributes) + "/Type")]
public class BaseAttribute : A_Attribute<BaseAttribute, BaseAttributes>
{
    public static string AttributeType = nameof(BaseAttribute);

    public override float Get(I_DeliveryTool deliveryTool)
    {
        ToolManager toolManager = (deliveryTool as DeliveryTool).toolManager;
        if (toolManager)
        {
            BaseAttributeTool at = toolManager.Get<BaseAttributeTool>();
            if (at)
            {
                return at.GetAttribute(this);
            }
        }
        return 0;
    }

    public override string GetAttributeType()
    {
        return AttributeType;
    }
}
