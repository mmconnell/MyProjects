using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public interface I_TagOperation
    {
        void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments);
        string visualize(int depth);
    }
}