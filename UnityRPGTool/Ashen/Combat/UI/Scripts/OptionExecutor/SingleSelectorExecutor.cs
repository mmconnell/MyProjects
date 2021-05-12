using UnityEngine;
using System.Collections;
using Manager;

public class SingleSelectorExecutor : MonoBehaviour, I_OptionExecutor
{
    public A_Ability ability;

    public void ExecuteOption(ToolManager source)
    {
        PlayerInputState.Instance.chosenAbility = ability;
    }
}
