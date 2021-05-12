using Manager;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultValues", menuName = "Custom/CombatInfrastructure/DefaultValues")]
public class DefaultValues : SingletonScriptableObject<DefaultValues>
{
    public AttributeToolConfiguration defaultAttributeToolConfiguration;
    public BaseAttributeToolConfiguration defaultBaseAttributeToolConfiguration;
    public ResistanceToolConfiguration defaultResistanceToolConfiguration;
    public StatusToolConfiguration defaultStatusToolConfiguration;
    public InfoCanvasToolConfiguration defaultInfoCanvasToolConfiguration;
    public LevelToolConfiguration defaultLevelToolConfiguration;
    public GemGeneratorConfiguration defaultGemGeneratorConfiguration;
    public GemDropperConfiguration defaultGemDropperConfiguration;
    public ResourceValueToolConfiguration defaultResourceValueToolConfiguration;
}
