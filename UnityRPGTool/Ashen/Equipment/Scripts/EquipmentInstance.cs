using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using System.Collections.Generic;

namespace Ashen.EquipmentSystem
{
    public class EquipmentInstance
    {
        private string name;
        private List<I_ExtendedEffect> effects;
        private EquipmentRarity rarity;
        private EquipmentType type;

        //EquipmentImage

        public EquipmentInstance(string name, List<I_ExtendedEffect> effects, EquipmentRarity rarity, EquipmentType type)
        {
            this.name = name;
            this.effects = effects;
            this.rarity = rarity;
            this.type = type;
        }
    }
}