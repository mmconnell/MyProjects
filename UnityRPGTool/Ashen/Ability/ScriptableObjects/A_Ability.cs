using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Ashen.DeliverySystem;

public abstract class A_Ability : SerializedScriptableObject
{
    [PropertyRange(0, 100)]
    public int speedFactor;
    public TargetParty targetParty;

    public abstract Target GetTargetType();
    public abstract void ResolveTarget(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder);
    public  virtual void ProcessTargetDisplay(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder, I_Targetable previous, I_Targetable current)
    {

    }
    public abstract void Execute(A_PartyManager sourceParty, A_PartyManager targetParty, ActionHolder actionHolder);
}

public enum TargetParty
{
    PLAYER, ENEMY
}