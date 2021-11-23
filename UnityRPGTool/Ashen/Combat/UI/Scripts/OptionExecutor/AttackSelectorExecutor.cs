using UnityEngine;
using System.Collections;
using Manager;

public class AttackSelectorExecutor : MonoBehaviour, I_OptionExecutor
{
    public CombatOptionUI combatOption;

    public void ExecuteOption(ToolManager source)
    {
        AbilityHolder abilityHolder = source.Get<AbilityHolder>();
        PlayerInputState.Instance.chosenAbility = abilityHolder.AttackAbility;
    }

    public void InitializeOption(ToolManager source)
    {
        combatOption.Valid = true;
    }
}
