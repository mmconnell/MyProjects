using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using Manager;

namespace Ashen.DeliverySystem
{
    public class InterruptActionEffect : I_Effect, ISerializable
    {
        private AbilitySO abilitySO;
        private AbilityTag[] abilityTags;

        public InterruptActionEffect() { }

        public InterruptActionEffect(AbilitySO ability, AbilityTag[] abilityTags)
        {
            this.abilitySO = ability;
            this.abilityTags = abilityTags;
        }

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            ToolManager targetTM = (target as DeliveryTool).toolManager;
            if (ExecuteInputState.Instance.currentSubactionProcessor != null)
            {
                SubactionProcessor processor = ExecuteInputState.Instance.currentSubactionProcessor;
                if (targetTM != processor.actionExecutable.target)
                {
                    return;
                }
                Ability ability = abilitySO.abilityBuilder.BuildAbility();
                AbilityAction abilityAction = ability.primaryAbilityAction;
                Target targetSO = abilityAction.GetTargetType(processor.actionExecutable.source);
                I_TargetHolder targetHolder = targetSO.BuildTargetHolder();
                A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;
                A_PartyManager enemyParty = EnemyPartyHolder.Instance.enemyPartyManager;
                
                A_PartyManager sourceParty;
                A_PartyManager targetParty;
                PartyPosition position = playerParty.GetPosition(targetTM);
                if (position != null)
                {
                    sourceParty = playerParty;
                }
                else
                {
                    sourceParty = enemyParty;
                }
                position = playerParty.GetPosition(processor.actionExecutable.source);
                if (position != null)
                {
                    targetParty = playerParty;
                }
                else
                {
                    targetParty = enemyParty;
                }
                ActionProcessor actionProcessor = new ActionProcessor(abilityAction, targetTM, sourceParty, targetParty, targetHolder);
                targetHolder.SetTargetable(targetTM, processor.actionExecutable.source, sourceParty, targetParty, actionProcessor);
                ExecuteInputState.Instance.AddInturruptAction(actionProcessor);
            }
        }

        protected InterruptActionEffect(SerializationInfo info, StreamingContext context)
        {
            int length = info.GetInt32(nameof(abilityTags) + "-Count");
            abilityTags = new AbilityTag[length];
            for (int x = 0; x < length; x++)
            {
                abilityTags[x] = AbilityTags.Instance[info.GetInt32(nameof(abilityTags) + "-" + x)];
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(abilityTags) + "-Count", abilityTags.Length);
            for (int x = 0; x < abilityTags.Length; x++)
            {
                info.AddValue(nameof(abilityTags) + "-" + x, (int)abilityTags[x]);
            }
        }
    }
}
