using Ashen.EquationSystem;
using Manager;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using UnityEditor;

namespace Ashen.DeliverySystem
{
    /**
     * The StatusEffect is an Effect that wraps a StatusEffectScriptableObject
     * and holds the TickerPack which defines how long the StatusEffect will last
     * and how often it will 'tick' (if applicable). 
     * A tick is a triggered effect i.e. DamageOverTime 'ticks' every time it applies damage
     **/
     [Serializable]
    public class StatusEffectPack : I_Effect, ISerializable
    {
        public StatusEffectScriptableObject Copy;
        public I_Ticker ticker;

        public StatusEffectPack(StatusEffectScriptableObject copy, I_Ticker ticker)
        {
            this.Copy = copy;
            this.ticker = ticker;
        }

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetResultPack, DeliveryArgumentPacks deliveryArguments)
        {
            Logger.DebugLog("Applying " + this.ToString());
            I_ExtendedEffect statusEffect = Copy.Clone(owner, target, deliveryArguments);
            if (ticker != null)
            {
                statusEffect.SetTicker(ticker.Duplicate());
            }
            StatusEffectResult deliveryResult = targetResultPack.GetResult<StatusEffectResult>(DeliveryResultTypes.Instance.STATUS_EFFECT_RESULT_TYPE);
            deliveryResult.AppliedStatusEffects.Add(statusEffect);
            targetResultPack.empty = false;
        }

        public override string ToString()
        {
            return Copy.ToString();
        }

        public StatusEffectPack(SerializationInfo info, StreamingContext context)
        {
            Copy = (StatusEffectScriptableObject) AssetDatabase.LoadAssetAtPath(info.GetString(nameof(Copy)), typeof(StatusEffectScriptableObject));
            Type tickerType = Type.GetType(info.GetString(nameof(ticker) + nameof(Type)));
            ticker = (I_Ticker)info.GetValue(nameof(ticker), tickerType);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Copy), AssetDatabase.GetAssetPath(Copy));
            info.AddValue(nameof(ticker), ticker);
            info.AddValue(nameof(ticker) + nameof(Type), ticker.GetType().FullName);
        }
    }
}
