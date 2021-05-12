using Ashen.EquationSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    /**
     * A TickerPack must know how to create an instance of its corresponding
     * Ticker
     **/
    [Title("TICKER"), HideLabel, InlineProperty]
    public interface I_TickerPack
    {
        I_Ticker Build(I_DeliveryTool owner, I_DeliveryTool target, EquationArgumentPack extraArguments);
    }
}
