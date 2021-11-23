using Ashen.DeliverySystem;
using Manager;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * A DerivedStatusEffect contains a list of BaseStatusEffects, A TickerPack (duration
     * and frequency of status effect) An owner (Character who applied the status effect)
     * and a target (Receiver of status effect). Each BaseStatusEffect will have a list
     * of triggers that tell it when it can take action. For most cases, these BaseStatusEffects
     * will have a trigger of 'tick' which means they will apply an effect every time
     * that the TickerPack 'ticks'. 
     **/
    public class ExtendedEffect : I_ExtendedEffect
    {
        [ReadOnly, ShowInInspector]
        private List<ExtendedEffectContainer> statusEffects;
        private bool disabled = false;

        public string key;

        [HideInInspector]
        public I_Ticker Ticker { get; set; }
        [HideInInspector]
        public I_DeliveryTool owner;
        [HideInInspector]
        public I_DeliveryTool target;
        [HideInInspector]
        private List<ExtendedEffectContainer>[] baseTriggers;
        [HideInInspector]
        public bool enabled = false;
        [HideInInspector]
        public bool[] listsIncluded = new bool[ExtendedEffectTriggers.Count];
        [HideInInspector]
        public DeliveryContainer deliveryContainer;
        [HideInInspector]
        public List<ExtendedEffectTag> tags;
        [HideInInspector]
        public bool savable;

        private DeliveryArgumentPacks arguments;

        public ExtendedEffect() { }

        public void RebuildExtendedEffect(List<ExtendedEffectContainer> statusEffects, List<ExtendedEffectTag> tags, string key, I_DeliveryTool target)
        {
            disabled = false;
            this.target = target;
            this.tags = tags;
            this.key = key;
            this.statusEffects = statusEffects;
            baseTriggers = new List<ExtendedEffectContainer>[ExtendedEffectTriggers.Count];
            for (int x = 0; x < baseTriggers.Length; x++)
            {
                baseTriggers[x] = new List<ExtendedEffectContainer>();
            }
            for (int x = 0; x < statusEffects.Count; x++)
            {
                ExtendedEffectContainer statusEffect = statusEffects[x];
                ExtendedEffectTrigger[] statusTriggers = statusEffect.statusEffect.GetStatusTriggers();
                foreach (ExtendedEffectTrigger statusTrigger in statusTriggers)
                {
                    List<ExtendedEffectContainer> list = baseTriggers[statusTrigger.Index];
                    list.Add(statusEffect);
                }
            }
        }

        public ExtendedEffect(List<I_ComponentBuilder> baseStatusEffects, List<ExtendedEffectTag> tags, string key, I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments, bool savable = true)
        {
            disabled = false;
            this.owner = owner;
            this.target = target;
            this.tags = new List<ExtendedEffectTag>();
            if (tags != null)
            {
                this.tags.AddRange(tags);
            }
            long milliseconds = Environment.TickCount;
            this.key = key + " - " + milliseconds;
            statusEffects = new List<ExtendedEffectContainer>();
            baseTriggers = new List<ExtendedEffectContainer>[ExtendedEffectTriggers.Count];
            for (int x = 0; x < baseTriggers.Length; x++)
            {
                baseTriggers[x] = new List<ExtendedEffectContainer>();
            }
            this.arguments = deliveryArguments;
            for (int x = 0; x < baseStatusEffects.Count; x++) 
            {
                I_ComponentBuilder statusEffect = baseStatusEffects[x];
                AddBaseStatusEffect(statusEffect, deliveryArguments);
            }
        }

        public virtual void Enable()
        {
            if (disabled)
            {
                throw new Exception("Cannot enable a status effect that has been disabled");
            }
            if (!enabled)
            {
                StatusTool statusTool = ((DeliveryTool)target).toolManager.Get<StatusTool>();
                statusTool.RegisterStatusEffect(this);
                foreach(ExtendedEffectTag tag in tags)
                {
                    statusTool.AddStatusEffectTag(tag, this);
                }
                bool[] toAddTo = new bool[ExtendedEffectTriggers.Count];
                if (Ticker != null)
                {
                    if (TimeRegistry.Instance.turnBased)
                    {
                        if (!toAddTo[(int)ExtendedEffectTriggers.Instance.TurnStart] && !listsIncluded[(int)ExtendedEffectTriggers.Instance.TurnStart])
                        {
                            toAddTo[(int)ExtendedEffectTriggers.Instance.TurnStart] = true;
                            statusTool.RegisterStatusEffectTrigger(ExtendedEffectTriggers.Instance.TurnStart, this);
                        }
                    }
                    else
                    {
                        if (!toAddTo[(int)ExtendedEffectTriggers.Instance.Tick] && !listsIncluded[(int)ExtendedEffectTriggers.Instance.Tick])
                        {
                            toAddTo[(int)ExtendedEffectTriggers.Instance.Tick] = true;
                            statusTool.RegisterStatusEffectTrigger(ExtendedEffectTriggers.Instance.Tick, this);
                        }
                    }
                }
                foreach (ExtendedEffectContainer container in statusEffects)
                {
                    container.statusEffect.Apply(this, container);
                    foreach (ExtendedEffectTrigger statusTrigger in container.statusEffect.GetStatusTriggers())
                    {
                        if (!toAddTo[statusTrigger.Index] && !listsIncluded[statusTrigger.Index])
                        {
                            toAddTo[statusTrigger.Index] = true;
                            statusTool.RegisterStatusEffectTrigger(statusTrigger, this);
                        }
                    }
                }
                enabled = true;
            }
            if (Ticker != null)
            {
                Ticker.Enable(this);
            }
        }

        public virtual void Disable(bool natural)
        {
            if (enabled)
            {
                disabled = true;
                StatusTool statusTool = ((DeliveryTool)target).toolManager.Get<StatusTool>();
                if (statusTool)
                {
                    statusTool.UnRegisterStatusEffect(this);
                }
                for (int x = 0; x < statusEffects.Count; x++)
                {
                    ExtendedEffectContainer container = statusEffects[x];
                    if (natural)
                    {
                        container.statusEffect.End(this, container);
                    }
                    else
                    {
                        container.statusEffect.Remove(this, container);
                    }
                    if (container.type == ExtendedEffectType.TEMPORARY)
                    {
                        statusEffects.RemoveAt(x);
                        x--;
                    }
                }
                
                if (this.tags != null)
                {
                    foreach (ExtendedEffectTag tag in tags)
                    {
                        if (statusTool)
                        {
                            statusTool.RemoveStatusEffectTag(tag, this);
                        }
                    }
                }
                if (arguments != null)
                {
                    arguments.Disable();
                }
                enabled = false;
            }
            if (Ticker != null)
            {
                Ticker.Disable();
            }
        }

        public virtual void Trigger(ExtendedEffectTrigger statusTrigger)
        {
            if (TimeRegistry.Instance.turnBased && statusTrigger == ExtendedEffectTriggers.Instance.TurnStart && Ticker != null)
            {
                Ticker.UpdateTime();
            }
            Trigger(baseTriggers[statusTrigger.Index], statusTrigger);
        }

        public virtual void Remove()
        {
            Disable(false);
        }

        public virtual I_DeliveryTool Target()
        {
            return target;
        }

        private void Trigger(List<ExtendedEffectContainer> list, ExtendedEffectTrigger statusTrigger)
        {
            if (list.Count == 0)
            {
                return;
            }
            deliveryContainer = PoolManager.Instance.deliveryContainerPool.GetObject();
            foreach (ExtendedEffectContainer container in list)
            {
                container.statusEffect.Trigger(this, statusTrigger, container);
            }
            if (!deliveryContainer.Empty())
            {
                DeliveryArgumentPacks deliveryArguments = PoolManager.Instance.deliveryArgumentsPool.GetObject();
                if (arguments != null)
                {
                    EffectsArgumentPack sourceEffectArguments = arguments.GetPack<EffectsArgumentPack>();
                    EffectsArgumentPack newEffectArguments = deliveryArguments.GetPack<EffectsArgumentPack>();
                    newEffectArguments.CopyFloatArguments(sourceEffectArguments);
                }
                DeliveryUtility.Deliver(deliveryContainer, owner, target, deliveryArguments);
                deliveryContainer.Clear();
                deliveryArguments.Disable();
            }
            deliveryContainer.Disable();
            deliveryContainer = null;
        }

        public virtual ExtendedEffectContainer AddBaseStatusEffect(I_ComponentBuilder baseStatusEffect, DeliveryArgumentPacks deliveryArguments)
        {
            return AddStatusEffect(baseStatusEffect, ExtendedEffectType.NORMAL, deliveryArguments);
        }

        public virtual ExtendedEffectContainer AddStatusEffect(I_ComponentBuilder statusEffect, ExtendedEffectType type, DeliveryArgumentPacks deliveryArguments)
        {
            I_ExtendedEffectComponent createdStatusEffect = statusEffect.Build(owner, target, deliveryArguments);
            if (createdStatusEffect == null)
            {
                return new ExtendedEffectContainer();
            }
            long milliseconds = Environment.TickCount;
            string key = this.key + " - " + milliseconds;
            ExtendedEffectContainer container = new ExtendedEffectContainer(createdStatusEffect, key, type);
            ExtendedEffectTrigger[] statusTriggers = createdStatusEffect.GetStatusTriggers();
            foreach (ExtendedEffectTrigger statusTrigger in statusTriggers)
            {
                List<ExtendedEffectContainer> list = baseTriggers[statusTrigger.Index];
                list.Add(container);
            }
            statusEffects.Add(container);
            return container;
        }

        public virtual string GetName()
        {
            return GetType().Name;
        }

        public virtual void SetTicker(I_Ticker ticker)
        {
            Ticker = ticker;
        }

        public void Tick()
        {
            Trigger(ExtendedEffectTriggers.Instance.Tick);
        }

        public void UpdateTime()
        {
            Trigger(ExtendedEffectTriggers.Instance.UpdateTick);
        }

        public void End()
        {
            Disable(true);
        }

        public ExtendedEffectState CaptureState()
        {
            List<I_ExtendedEffectComponent> components = new List<I_ExtendedEffectComponent>();
            List<string> keys = new List<string>();
            List<int> tagValues = new List<int>();
            foreach (ExtendedEffectContainer container in statusEffects)
            {
                components.Add(container.statusEffect);
                keys.Add(container.key);
            }
            foreach (ExtendedEffectTag tag in tags)
            {
                tagValues.Add((int)tag);
            }
            return new ExtendedEffectState
            {
                keys = keys,
                baseStatusEffects = components,
                tagValues = tagValues,
                key = key,
                ticker = Ticker
            };
        }

        [Serializable]
        public struct ExtendedEffectState
        {
            public List<string> keys;
            public List<I_ExtendedEffectComponent> baseStatusEffects;
            public List<int> tagValues;
            public string key;
            public I_Ticker ticker;
        }
    }
}