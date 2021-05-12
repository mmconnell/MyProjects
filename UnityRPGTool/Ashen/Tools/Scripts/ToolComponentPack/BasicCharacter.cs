using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class BasicCharacter : MonoBehaviour
    {
        public AttributeToolConfiguration attributeToolConfiguration;
        public ResistanceToolConfiguration resistanceToolConfiguration;
        public StatusToolConfiguration statusToolConfiguration;
        public BaseAttributeToolConfiguration baseAttributeToolConfiguration;
        public ResourceValueToolConfiguration resourceValueToolConfiguration;

        private void Awake()
        {
            if (!gameObject.GetComponent<DeliveryTool>())
            {
                gameObject.AddComponent<DeliveryTool>();
            }
            if (!gameObject.GetComponent<DamageTool>())
            {
                gameObject.AddComponent<DamageTool>();
            }
            if (!gameObject.GetComponent<AttributeTool>())
            {
                AttributeTool attributeTool = gameObject.AddComponent<AttributeTool>();
                attributeTool.Initialize(attributeToolConfiguration);
            }
            if (!gameObject.GetComponent<ResistanceTool>())
            {
                ResistanceTool resistanceTool = gameObject.AddComponent<ResistanceTool>();
                resistanceTool.Initialize(resistanceToolConfiguration);
            }
            if (!gameObject.GetComponent<StatusTool>())
            {
                StatusTool statusTool = gameObject.AddComponent<StatusTool>();
                statusTool.Initialize(statusToolConfiguration);
            }
            if (!gameObject.GetComponent<BaseAttributeTool>())
            {
                BaseAttributeTool baseAttributeTool = gameObject.AddComponent<BaseAttributeTool>();
                baseAttributeTool.Initialize(baseAttributeToolConfiguration);
            }
            if (!gameObject.GetComponent<ResourceValueTool>())
            {
                ResourceValueTool resourceValueTool = gameObject.AddComponent<ResourceValueTool>();
            }
            if (!gameObject.GetComponent<TriggerTool>())
            {
                TriggerTool triggerTOol = gameObject.AddComponent<TriggerTool>();
            }
        }
    }
}
