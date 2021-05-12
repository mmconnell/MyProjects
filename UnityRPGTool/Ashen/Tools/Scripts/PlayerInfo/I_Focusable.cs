using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;

namespace Manager
{
    public interface I_Focusable
    {
        string BuildString(ToolManager deliveryTool);
        bool HandleFocus(ToolManager deliveryTool);
    }
}