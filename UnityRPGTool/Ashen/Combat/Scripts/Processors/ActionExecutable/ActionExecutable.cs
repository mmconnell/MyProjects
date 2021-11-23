using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Manager;

public class ActionExecutable : I_ActionExecutable
{
    public DeliveryPackBuilder builder;
    public ToolManager source;
    public ToolManager target;
    public I_AbilityAction sourceAbility;
    public float?[] effectFloatArguments;
    //public DeliveryArgumentPacks deliveryArgumentPacks;

    public bool retargeted;

    private bool isFinished = false;

    public IEnumerator Execute(MonoBehaviour runner)
    {
        DeliveryArgumentPacks deliveryArgumentPacks = PoolManager.Instance.deliveryArgumentsPool.GetObject();
        sourceAbility.FillDeliveryArguments(deliveryArgumentPacks);
        if (effectFloatArguments != null)
        {
            EffectsArgumentPack effectArgumentsPack = deliveryArgumentPacks.GetPack<EffectsArgumentPack>();
            foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
            {
                if (effectFloatArguments[(int)argument] != null)
                {
                    effectArgumentsPack.SetFloatArgument(argument, ((float)effectFloatArguments[(int)argument]));
                }
            }
        }
        DeliveryContainer container = PoolManager.Instance.deliveryContainerPool.GetObject();
        DeliveryTool sDT = source.Get<DeliveryTool>();
        DeliveryTool tDT = target.Get<DeliveryTool>();
        container.AddPrimaryEffect(builder.deliveryPack.Build(sDT, tDT, deliveryArgumentPacks));
        if (builder.preFilters != null)
        {
            container.AddPreFilter(new KeyContainer<I_Filter>() {
                source = builder.preFilters.Build(sDT, tDT, deliveryArgumentPacks),
            });
        }
        if (builder.postFilters != null)
        {
            container.AddPostFilter(new KeyContainer<I_Filter>()
            {
                source = builder.postFilters.Build(sDT, tDT, deliveryArgumentPacks),
            });
        }
        DeliveryUtility.Deliver(container, source.Get<DeliveryTool>(), target.Get<DeliveryTool>(), deliveryArgumentPacks);
        TriggerTool triggerTool = target.Get<TriggerTool>();
        foreach (AbilityTag tag in sourceAbility.GetAbilityTags(source))
        {
            if (tag.effectTrigger)
            {
                triggerTool.Trigger(tag.effectTrigger);
            }
        }
        deliveryArgumentPacks.Disable();
        container.Disable();
        isFinished = true;
        yield break;
    }

    public bool IsFinished()
    {
        return isFinished;
    }
}
