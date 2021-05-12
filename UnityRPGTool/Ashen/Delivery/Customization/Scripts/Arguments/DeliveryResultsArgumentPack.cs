using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public class DeliveryResultsArgumentPack : A_DeliveryArgumentPack<DeliveryResultsArgumentPack>
    { 
        private DeliveryResultPack pack;

        public DeliveryResultsArgumentPack()
        {
            pack = new DeliveryResultPack();
        }

        public override I_DeliveryArgumentPack Initialize()
        {
            return new DeliveryResultsArgumentPack();
        }

        public DeliveryResultPack GetDeliveryResultPack()
        {
            return pack;
        }

        public override void Clear()
        {
            pack.Clear();
        }
    }
}