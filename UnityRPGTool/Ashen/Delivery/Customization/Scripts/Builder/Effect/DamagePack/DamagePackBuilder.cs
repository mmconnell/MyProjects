using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Ashen.VariableSystem;
using Ashen.EquationSystem;
using System.Collections.Generic;
using Manager;

namespace Ashen.DeliverySystem
{
    [InlineProperty]
    public class DamagePackBuilder : I_EffectBuilder
    {
        [HideLabel, EnumToggleButtons, OdinSerialize]
        private DamageTypeOption option;
        [OdinSerialize]
        private bool useWeapon;
        [OdinSerialize, HideLabel, EnumSODropdown]
        [ShowIf("@" + nameof(option) + " == " + nameof(DamageTypeOption) + "." + nameof(DamageTypeOption.Singular) + 
            " && !" + nameof(useWeapon))]
        private DamageType damageType = default;
        [OdinSerialize, EnumSODropdown, HideLabel, Title("Enabled Damage Types")]
        [ShowIf("@" + nameof(option) + " == " + nameof(DamageTypeOption) + "." + nameof(DamageTypeOption.Collection) +
            " && !" + nameof(useWeapon))]
        private List<DamageType> damageTypes = default;
        [OdinSerialize, Hide]
        private ScalingValueBuilder value;

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            EffectsArgumentPack effectArgs = deliveryArguments.GetPack<EffectsArgumentPack>();
            float damageScale = effectArgs.GetFloatScale(EffectFloatArguments.Instance.reservedDamageScale);
            float total = damageScale * value.Build(owner, target, deliveryArguments);
            DamageType newDamageType = null;
            if (option == DamageTypeOption.Singular)
            {
                if (useWeapon)
                {
                    EquipmentTool ownerEt = (owner as DeliveryTool).toolManager.Get<EquipmentTool>();
                    List<DamageType> damageTypes = ownerEt.GetWeaponDamageTypes();
                    if (damageTypes == null || damageTypes.Count <= 0)
                    {
                        Logger.ErrorLog("Weapon damage type could not be resolved, defaulting to normal damage type");
                        newDamageType = DamageTypes.Instance.NORMAL;
                    }
                    newDamageType = damageTypes[0];
                }
                else
                {
                    newDamageType = damageType;
                }
                return new DamagePack(newDamageType, total);
            }
            else
            {
                List<DamageType> newDamageTypes = new List<DamageType>();
                if (useWeapon)
                {
                    EquipmentTool ownerEt = (owner as DeliveryTool).toolManager.Get<EquipmentTool>();
                    List<DamageType> damageTypes = ownerEt.GetWeaponDamageTypes();
                    if (damageTypes == null || damageTypes.Count <= 0)
                    {
                        Logger.ErrorLog("Weapon damage type could not be resolved, defaulting to normal damage type");
                        newDamageTypes.Add(DamageTypes.Instance.NORMAL);
                    }
                    newDamageTypes.AddRange(damageTypes);
                }
                else
                {
                    newDamageTypes.AddRange(damageTypes);
                }
                return new CollectiveDamagePack(newDamageTypes, total);
            }
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += "Deal [" + value.ToString() + "] OF ";
            if (useWeapon)
            {
                vis += " weapon's damage type";
            }
            else
            {
                vis += damageType.ToString();
            }
            return vis;
        }
    }

    public enum DamageTypeOption
    {
        Singular, Collection
    }
}