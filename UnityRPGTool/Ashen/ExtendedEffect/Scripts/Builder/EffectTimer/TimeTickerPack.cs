using Ashen.EquationSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using Ashen.VariableSystem;

namespace Ashen.DeliverySystem
{
    /**
     * A TimeTickerPack knows how to create an instance of 
     * a TimeTicker. The difference between the two, is that
     * a status effect will have a TimeTickerPack, and whenever
     * the status is applied, that time ticker pack will build
     * a Ticker that will manage the StatusEffect for the individual
     * character it is applied to. I.E. if the same Status effect
     * is applied to 5 characters, then there are 5 Tickers that 
     * are created.
     **/
     [Indent]
    public class TimeTickerPack : I_TickerPack
    {
        [HorizontalGroup("TimeTicker"), Title("Duration"), OdinSerialize, HideLabel]
        private Reference<I_Equation> duration;
        [HorizontalGroup("TimeTicker"), Title("Frequency"), OdinSerialize, HideLabel]
        private Reference<I_Equation> frequency;

        public TimeTickerPack() { }
        public TimeTickerPack(Reference<I_Equation> duration, Reference<I_Equation> frequency)
        {
            this.duration = duration;
            this.frequency = frequency;
        }

        public I_Ticker Build(I_DeliveryTool owner, I_DeliveryTool target, EquationArgumentPack extraArguments)
        {
            if (duration == null || duration.Value == null)
            {
                return new TimeTicker(null, (int)frequency.Value.Calculate(owner, target, extraArguments), TimeRegistry.Instance.turnBased);
            }
            return new TimeTicker((int)duration.Value.Calculate(owner, target, extraArguments), (int)frequency.Value.Calculate(owner, target, extraArguments), TimeRegistry.Instance.turnBased);
        }
    }
}
