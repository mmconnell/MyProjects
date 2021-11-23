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
        public TargetAttributeToolConfiguration targetAttributeToolConfiguration;
        public AbilityHolderConfiguration abilityHolderConfiguration;
        public EquipmentToolConfiguration equipmentToolConfiguration;

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
                resourceValueTool.Initialize(resourceValueToolConfiguration);
            }
            if (!gameObject.GetComponent<TriggerTool>())
            {
                TriggerTool triggerTOol = gameObject.AddComponent<TriggerTool>();
            }
            if (!gameObject.GetComponent<TargetAttributeTool>())
            {
                TargetAttributeTool targetTool = gameObject.AddComponent<TargetAttributeTool>();
                targetTool.Initialize(targetAttributeToolConfiguration);
            }
            if (!gameObject.GetComponent<AbilityHolder>())
            {
                AbilityHolder abilityHolder = gameObject.AddComponent<AbilityHolder>();
                abilityHolder.Initialize(abilityHolderConfiguration);
            }
            if (!gameObject.GetComponent<EquipmentTool>())
            {
                EquipmentTool equipmentTool = gameObject.AddComponent<EquipmentTool>();
                equipmentTool.Initialize(equipmentToolConfiguration);
            }
        }
    }
}
