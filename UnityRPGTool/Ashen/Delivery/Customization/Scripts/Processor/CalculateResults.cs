using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public class CalculateResults : I_DeliveryProcessor
    {
        public void process(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryResultsArgumentPack resultPack = deliveryArguments.GetPack<DeliveryResultsArgumentPack>();
            resultPack.GetDeliveryResultPack().Calculate(owner, target, deliveryArguments);
        }
    }
}