using System.Collections;
using System.Collections.Generic;
using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using Manager;
using Sirenix.OdinInspector;
using UnityEngine;

/**
 * A character attribute defines the core values that make up the general power and effectiveness of a character
 **/
[CreateAssetMenu(fileName = nameof(DerivedAttribute), menuName = "Custom/Enums/" + nameof(DerivedAttributes) + "/Type")]
public class DerivedAttribute : A_Attribute<DerivedAttribute, DerivedAttributes, float>
{
    public static string AttributeType = nameof(DerivedAttribute);
    public Equation equation;

    public override float Get(I_DeliveryTool deliveryTool)
    {
        ToolManager toolManager = (deliveryTool as DeliveryTool).toolManager;
        if (toolManager)
        {
            AttributeTool at = toolManager.Get<AttributeTool>();
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
