using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using static Ashen.DeliverySystem.ExtendedEffect;

namespace Manager
{
    /**
     * The status tool is used to manage all the status effects that are effecting an individual character
     **/
    public class StatusTool : A_EnumeratedTool<StatusTool>, I_ThresholdListener, I_Saveable, I_TriggerListener
    {
        private List<ExtendedEffect>[] statusEvents;
        private HashSet<ExtendedEffect>[] statusTagGroups;
        [ShowInInspector]
        private List<ExtendedEffect> extendedEffects;

        [OdinSerialize]
        private StatusToolConfiguration statusToolConfiguration = default;
        private StatusToolConfiguration StatusToolConfiguration
        {
            get
            {
                if (statusToolConfiguration == null)
                {
                    return DefaultValues.Instance.defaultStatusToolConfiguration;
                }
                return statusToolConfiguration;
            }
        }
        private DeliveryPackScriptableObject[] statusEffectResults;

        public DeliveryResultPack currentPack;

        public void Initialize(StatusToolConfiguration config)
        {
            statusToolConfiguration = config;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            statusEffectResults = StatusToolConfiguration.StatusEffectResults;
            statusEvents = new List<ExtendedEffect>[ExtendedEffectTriggers.Count];
            for (int x = 0; x < ExtendedEffectTriggers.Count; x++)
            {
                statusEvents[x] = new List<ExtendedEffect>();
            }
            statusTagGroups = new HashSet<ExtendedEffect>[ExtendedEffectTags.Count];
            for (int x = 0; x < ExtendedEffectTags.Count; x++)
            {
                statusTagGroups[x] = new HashSet<ExtendedEffect>();
            }
            extendedEffects = new List<ExtendedEffect>();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            foreach (ExtendedEffectTag tag in ExtendedEffectTags.Instance)
            {
                DisableAllStatusEffectsWithTag(tag);
            }
            ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                Logger.DebugLog("DamageType being registered: " + resourceValue);
                resourceValueTool.UnRegesterThresholdMetListener(resourceValue, this);
            }
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            foreach (ExtendedEffectTrigger trigger in ExtendedEffectTriggers.Instance)
            {
                triggerTool.UnregisterTriggerListener(trigger, this);
            }
        }

        public void Start()
        {
            ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                Logger.DebugLog("DamageType being registered: " + resourceValue);
                resourceValueTool.RegisterThresholdMetListener(resourceValue, this);
            }
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            foreach (ExtendedEffectTrigger trigger in ExtendedEffectTriggers.Instance)
            {
                triggerTool.RegisterTriggerListener(trigger, this);
            }
        }

        public void RegisterStatusEffect(ExtendedEffect extendedEffect)
        {
            extendedEffects.Add(extendedEffect);
        }

        public void UnRegisterStatusEffect(ExtendedEffect extendedEffect)
        {
            extendedEffects.Remove(extendedEffect);
        }

        public void RegisterStatusEffectTrigger(ExtendedEffectTrigger statusTrigger, ExtendedEffect derivedStatusEffect)
        {
            if (statusTrigger.shouldRegister)
            {
                if (!derivedStatusEffect.listsIncluded[statusTrigger.Index])
                {
                    derivedStatusEffect.listsIncluded[statusTrigger.Index] = true;
                    statusEvents[statusTrigger.Index].Add(derivedStatusEffect);
                }
            }
        }

        public void Trigger(ExtendedEffectTrigger statusTrigger)
        {
            List<ExtendedEffect> statusEffects = statusEvents[statusTrigger.Index];
            for (int x = 0; x < statusEffects.Count; x++)
            {
                ExtendedEffect dse = statusEffects[x];
                if (!dse.enabled)
                {
                    dse.listsIncluded[statusTrigger.Index] = false;
                    statusEffects.RemoveAt(x);
                    x--;
                }
                else
                {
                    dse.Trigger(statusTrigger);
                }
            }
        }

        public void AddStatusEffectTag(ExtendedEffectTag tag, ExtendedEffect derivedStatusEffect)
        {
            HashSet<ExtendedEffect> statusEffects = statusTagGroups[tag.Index];

            statusEffects.Add(derivedStatusEffect);
        }

        public void RemoveStatusEffectTag(ExtendedEffectTag tag, ExtendedEffect derivedStatusEffect)
        {
            HashSet<ExtendedEffect> statusEffects = statusTagGroups[tag.Index];
            statusEffects.Remove(derivedStatusEffect);
        }

        public bool CheckStatusEffectTag(ExtendedEffectTag tag)
        {
            HashSet<ExtendedEffect> statusEffects = statusTagGroups[tag.Index];
            return statusEffects.Count > 0;
        }

        public void DisableAllStatusEffectsWithTag(ExtendedEffectTag tag)
        {
            List<ExtendedEffect> statusEffectsCopy = new List<ExtendedEffect>(statusTagGroups[tag.Index]);
            foreach (ExtendedEffect derivedStatusEffect in statusEffectsCopy)
            {
                derivedStatusEffect.Disable(false);
            }
        }

        public void OnThresholdEvent(ThresholdEventValue value)
        {
            //DeliveryTool deliveryTool = toolManager.Get<DeliveryTool>();
            //DeliveryPackScriptableObject container = statusEffectResults[(int)value.damageType];
            //if (container)
            //{
            //    DeliveryContainer deliveryContainer = new DeliveryContainer();
            //    deliveryContainer.AddPrimaryEffect(statusEffectResults[(int)value.damageType].deliveryPack);
            //    DeliveryArgumentPacks packs = new DeliveryArgumentPacks();
            //    DeliveryUtility.Deliver(deliveryContainer, deliveryTool, deliveryTool, packs);
            //}
        }

        public void OnTrigger(ExtendedEffectTrigger trigger)
        {
            Trigger(trigger);
        }

        public object CaptureState()
        {
            List<ExtendedEffectState> capturedStatusEffects = new List<ExtendedEffectState>();
            foreach (ExtendedEffect extendedEffect in extendedEffects)
            {
                if (extendedEffect.enabled && extendedEffect.savable)
                {
                    capturedStatusEffects.Add(extendedEffect.CaptureState());
                }
            }
            return new StatusSaveData
            {
                extendedEffectStates = capturedStatusEffects
            };
        }

        public void RestoreState(object state)
        {
            while (extendedEffects.Count > 0) 
            {
                ExtendedEffect effect = extendedEffects[0];
                effect.Disable(false);
            }
            StatusSaveData statusSaveData = (StatusSaveData)state;
            if (statusSaveData.extendedEffectStates != null)
            {
                foreach (ExtendedEffectState eState in statusSaveData.extendedEffectStates)
                {
                    ExtendedEffect effect = new ExtendedEffect();
                    List<ExtendedEffectContainer> containers = new List<ExtendedEffectContainer>();
                    List<ExtendedEffectTag> tags = new List<ExtendedEffectTag>();
                    foreach (int tagValue in eState.tagValues)
                    {
                        tags.Add(ExtendedEffectTags.Instance[tagValue]);
                    }
                    for (int x = 0; x < eState.baseStatusEffects.Count; x++)
                    {
                        I_ExtendedEffectComponent component = eState.baseStatusEffects[x];
                        string key = eState.keys[x];
                        containers.Add(new ExtendedEffectContainer(component, key));
                    }
                    effect.RebuildExtendedEffect(containers, tags, eState.key, toolManager.Get<DeliveryTool>());
                    effect.SetTicker(eState.ticker);
                    effect.Enable();
                }
            }
        }

        [Serializable]
        private struct StatusSaveData {
            public List<ExtendedEffectState> extendedEffectStates;
        }
    }
}
