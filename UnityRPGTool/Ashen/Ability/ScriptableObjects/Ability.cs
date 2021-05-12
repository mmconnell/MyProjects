using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;
using Sirenix.OdinInspector;

public class Ability : SerializedScriptableObject
{
    public TargetTypes targetType;
    [Hide, Title("Delivery Pack")]
    public DeliveryPackBuilder deliveryPack;
    public GameObject animation;
    public Target target;
}
