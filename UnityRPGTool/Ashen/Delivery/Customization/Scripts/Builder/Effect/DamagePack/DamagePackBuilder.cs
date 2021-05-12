using UnityEngine;
using System.Collections;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using Ashen.VariableSystem;
using Ashen.EquationSystem;

namespace Ashen.DeliverySystem
{
    [InlineProperty]
    public class DamagePackBuilder : I_EffectBuilder
    {
        [OdinSerialize]
        private bool useWeapon;
        [OdinSerialize, HideLabel, EnumSODropdown, HideIf(nameof(useWeapon))]
        private DamageType damageType = default;
        [HorizontalGroup(nameof(DamagePack), width: 0.5f), OdinSerialize, HideLabel]
        public Reference<I_Equation> equation = default;

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            if (useWeapon)
            {
                DamageType damageType = deliveryArguments.GetPack<EffectsArgumentPack>().WeaponDamageType;
                if (damageType == null)
                {
                    Logger.ErrorLog("Weapon damage type could not be resolved, defaulting to normal damage type");
                    return new DamagePack(DamageTypes.Instance.NORMAL, equation.Value.Calculate(owner, target, deliveryArguments.GetPack<EquationArgumentPack>()));
                }
                return new DamagePack(damageType, equation.Value.Calculate(owner, target, deliveryArguments.GetPack<EquationArgumentPack>()));
            }
            return new DamagePack(damageType, equation.Value.Calculate(owner, target, deliveryArguments.GetPack<EquationArgumentPack>()));
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < depth; x++)
            {
                vis += "\t";
            }
            vis += "Deal [" + equation.Value.ToString() + "] OF ";
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
}