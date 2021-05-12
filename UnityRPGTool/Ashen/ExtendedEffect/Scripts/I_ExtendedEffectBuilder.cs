using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public interface I_ExtendedEffectBuilder
    {
        I_ExtendedEffect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArgumentPacks);
    }
}