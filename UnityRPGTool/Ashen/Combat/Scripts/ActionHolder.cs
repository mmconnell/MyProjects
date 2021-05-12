using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Ashen.EquationSystem;

public class ActionHolder
{
    public ToolManager source;
    private float speed;
    private A_Ability sourceAbility;
    private A_PartyManager sourceParty;
    private A_PartyManager targetParty;
    public I_TargetHolder targetHodler;

    public ActionHolder(A_Ability sourceAbility, ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty)
    {
        this.sourceAbility = sourceAbility;
        this.source = source;
        DeliveryArgumentPacks packs = PoolManager.Instance.deliveryArgumentsPool.GetObject();
        speed = sourceAbility.speedFactor * Random.Range(1f, 10f) * Mathf.Max(0.1f, DerivedAttributes.GetEnum("Speed").equation.Calculate(source.Get<DeliveryTool>(), packs.GetPack<EquationArgumentPack>()));
    }

    public float GetSpeed()
    {
        return this.speed;
    }

    public void Resolve()
    {

    }

    public IEnumerator Execute()
    {
        sourceAbility.Execute(sourceParty, targetParty, this);
        yield return null;
    }
}
