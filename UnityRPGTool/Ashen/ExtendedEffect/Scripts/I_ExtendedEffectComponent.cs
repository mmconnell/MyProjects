using Manager;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    /**
     * All BaseStatusEffects are required to implement this interface
     **/
    public interface I_ExtendedEffectComponent
    {
        void Apply(ExtendedEffect dse, ExtendedEffectContainer container);
        void Remove(ExtendedEffect dse, ExtendedEffectContainer container);
        void End(ExtendedEffect dse, ExtendedEffectContainer container);
        void Trigger(ExtendedEffect dse, ExtendedEffectTrigger statusTrigger, ExtendedEffectContainer container);
        ExtendedEffectTrigger[] GetStatusTriggers();
        
    }
}
