using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public interface I_DeliveryProcessor
    {
        void process(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments);
    }
}