using UnityEngine;
using UnityEditor;

public interface I_StatusEffectSymbolListener
{
    void OnStatusEffectSymbolEvent(StatusEffectSymbolEventValue value);
}